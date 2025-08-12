using IdentityServer.Entities;
using IdentityService.Data;
using IdentityService.DTOs;
using IdentityService.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Services
{
    public class AutenticationService : IAuthentiactionService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _dbcontext;

        public AutenticationService(UserManager<User> userManager, IConfiguration configuration, ApplicationContext applicationContext)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _dbcontext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }


        public async Task<AutenticationModel> CreateAutenticationModel(User user)
        {
            var accessToken = await CreateToken(user);
            var refreshToken = await CreateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return new AutenticationModel { UserId = user.Id, AccessToken = accessToken, RefreshToken = refreshToken.Token };
        }

        private async Task<RefreshToken> CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            var token = new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiryTime = DateTime.Now.AddDays(Convert.ToDouble(_configuration.GetValue<string>("RefreshTokenExpires")))
            };

            _dbcontext.RefreshTokens.Add(token);
            await _dbcontext.SaveChangesAsync();

            return token;
        }

        private async Task<string> CreateToken(User user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var token = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var token = new JwtSecurityToken
            (
                issuer: jwtSettings.GetSection("validIssuer").Value,
                audience: jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
                signingCredentials: signingCredentials
            );

            return token;
        }

        private async Task<IEnumerable<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtSettings:secretKey"));
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<User> ValidateUser(UserCredentialsDto userCredentials)
        {
            var user = await _userManager.FindByNameAsync(userCredentials.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, userCredentials.Password))
            {
                return null;
            }
            return user;
        }

        public async Task RemoveRefreshToken(User user, string refreshToken)
        {
            user.RefreshTokens.RemoveAll(r => r.Token == refreshToken);
            await _userManager.UpdateAsync(user);

            var token = _dbcontext.RefreshTokens.FirstOrDefault(r => r.Token == refreshToken);
            if (token == null)
            {
                return;
            }

            _dbcontext.RefreshTokens.Remove(token);
            await _dbcontext.SaveChangesAsync();

        }
    }
}

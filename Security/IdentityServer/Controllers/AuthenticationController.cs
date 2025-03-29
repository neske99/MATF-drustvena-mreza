using AutoMapper;
using IdentityServer.Entities;
using IdentityService.Controllers.Base;
using IdentityService.DTOs;
using IdentityService.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticationController : RegistrationControllerBase
    {
        private readonly IAuthentiactionService _authenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IAuthentiactionService authenticationService)
            : base(logger, mapper, userManager, roleManager)
        {
            this._authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> RegisterBuyer([FromBody] NewUserDto newUser)
        {
            return await this.RegisterNewUserWithRoles(newUser, new string[] { "Buyer" });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> RegisterAdministrator([FromBody] NewUserDto newUser)
        {
            return await this.RegisterNewUserWithRoles(newUser, new string[] { "Admin" });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Login([FromBody] UserCredentialsDto userCredentials)
        {
            var user = await _authenticationService.ValidateUser(userCredentials);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(await _authenticationService.CreateAutenticationModel(user));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenModel refreshTokenModel)
        {
            var user = await _userManager.FindByNameAsync(refreshTokenModel.Username);
            if (user == null)
            {
                _logger.LogWarning($"{nameof(Refresh)}: Refreshing token failed. Unknown username {refreshTokenModel.Username}");
                return Forbid();
            }
            var refreshToken = user.RefreshTokens.FirstOrDefault(r => r.Token == refreshTokenModel.RefreshToken);
            if (refreshToken == null)
            {
                _logger.LogWarning($"{nameof(Refresh)}: Refreshing token failed. The refresh token is not found.");
                return Unauthorized();
            }
            if(refreshToken.ExpiryTime < DateTime.Now)
            {
                _logger.LogWarning($"{nameof(Refresh)}: Refreshing token failed. The refresh token is not valid.");
                return Unauthorized();
            }

            return Ok(await _authenticationService.CreateAutenticationModel(user));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenModel refreshTokenModel)
        {

            var user = await _userManager.FindByNameAsync(refreshTokenModel.Username);
            if (user == null)
            {
                _logger.LogWarning($"{nameof(Logout)}: Refreshing token failed. Unknown username {refreshTokenModel.Username}");
                return Forbid();
            }

            await _authenticationService.RemoveRefreshToken(user,refreshTokenModel.RefreshToken);
            return Accepted();
        }
    }
}
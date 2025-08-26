using AutoMapper;
using IdentityServer.Entities;
using IdentityServer.GrpcServices;
using IdentityService.DTOs;
using IdentityService.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IntRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly RelationsService _relationsService;
    public UserController(UserManager<User> userManager, RoleManager<IntRole> roleManager, IMapper mapper, RelationsService relationsService)
    {
      _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
      _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      _relationsService = relationsService ?? throw new ArgumentNullException(nameof(relationsService));
    }

    [HttpGet("GetAllUsers")]
        //[Authorize(Roles = Roles.Admin, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<UserDetails>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(int),  StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            //return Ok(users);
            return Ok(_mapper.Map<IEnumerable<UserDetails>>(users));
        }

        [HttpGet("GetSearchedUsers")]
        //[Authorize(Roles = Roles.Admin, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<UserDetails>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(int),  StatusCodes.Status200OK)]
        public async Task<ActionResult> GetSearchedUsersAsync([FromQuery] int userId,[FromQuery] string username)
        {
            var users = await _userManager.Users.Where(x => x.Id!=userId && x.UserName.Contains(username)).Take(100).ToListAsync();
            var userIdList=users.Select(u => u.Id).ToList();
            var relations= _relationsService.GetRelationShips(userId, userIdList);

            List<UserRelationDTO> userRelations = new List<UserRelationDTO>();

            for (int i = 0; i < users.Count; i++)
            {
                userRelations.Add(new UserRelationDTO
                {
                    Id = users[i].Id,
                    Username = users[i].UserName ?? string.Empty,
                    Relation = relations[i]
                });

            }

            //return Ok(users);
            return Ok(userRelations);
        }

        [HttpGet("GetFriendRequests")]
        public async Task<ActionResult> GetFriendRequests([FromQuery] int userId)
        {
            var friendRequestUserIdList = _relationsService.GetFriendRequestUserIdList(userId);
            var users = await _userManager.Users.Where(u => friendRequestUserIdList.Contains(u.Id)).ToListAsync();

            return Ok(
                    users.Select(u => new UserRelationDTO
                    {
                        Id = u.Id,
                        Username = u.UserName ?? string.Empty,
                        Relation = "RECEIVED_FRIENDSHIP_REQUEST_FROM"
                    }).ToList()

            );
        }


        [HttpGet("GetUser/{username}")]
        //[Authorize(Roles = Roles.Admin + "," + Roles.Buyer,AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<UserDetails>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetUserAsync(string username)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName == username);
            //return Ok(user);
            return Ok(_mapper.Map<UserDetails>(user));

        }

        [HttpPut("UpdateProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(UserDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateProfileAsync([FromBody] UpdateProfileDto updateProfile)
        {
            // Try multiple claim types to find the user ID
            var userId = User.FindFirst("nameid")?.Value ?? 
                         User.FindFirst("sub")?.Value ?? 
                         User.FindFirst("id")?.Value ?? 
                         User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
             
            if (string.IsNullOrEmpty(userId))
            {
                // Debug: Log all available claims
                var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
                return BadRequest($"User ID not found in token. Available claims: {string.Join(", ", claims.Select(c => $"{c.Type}={c.Value}"))}");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Check if username is being changed and if it's already taken
            if (!string.IsNullOrEmpty(updateProfile.Username) && updateProfile.Username != user.UserName)
            {
                var existingUser = await _userManager.FindByNameAsync(updateProfile.Username);
                if (existingUser != null)
                {
                    return BadRequest("Username is already taken");
                }
                user.UserName = updateProfile.Username;
                user.NormalizedUserName = updateProfile.Username.ToUpper();
            }

            // Update profile fields
            if (!string.IsNullOrEmpty(updateProfile.FirstName))
            {
                user.FirstName = updateProfile.FirstName;
            }

            if (!string.IsNullOrEmpty(updateProfile.LastName))
            {
                user.LastName = updateProfile.LastName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(_mapper.Map<UserDetails>(user));
            }

            return BadRequest(result.Errors);
        }

        [HttpPut("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto changePassword)
        {
            // Try multiple claim types to find the user ID
            var userId = User.FindFirst("nameid")?.Value ?? 
                         User.FindFirst("sub")?.Value ?? 
                         User.FindFirst("id")?.Value ?? 
                         User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                         
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID not found in token");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);
            if (result.Succeeded)
            {
                return Ok("Password changed successfully");
            }

            return BadRequest(result.Errors);
        }


    }
}

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


    }
}

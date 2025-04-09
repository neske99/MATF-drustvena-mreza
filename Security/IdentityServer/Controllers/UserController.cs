using AutoMapper;
using IdentityServer.Entities;
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
    [Authorize]
    public class UserController: ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IntRole> _roleManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, RoleManager<IntRole> roleManager, IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<UserDetails>),StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserDetails>>(users));
        }

        [HttpGet("{username}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Buyer,AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<UserDetails>),StatusCodes.Status200OK)]
        public async Task<ActionResult> GetUserAsync(string username)
        {
            var user =await  _userManager.Users.FirstOrDefaultAsync(user => user.UserName == username);

            return Ok(_mapper.Map<UserDetails>(user));
        
        }
    }
}

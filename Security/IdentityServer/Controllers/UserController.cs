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

        [HttpGet("GetAllUsers")]
        //[Authorize(Roles = Roles.Admin, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<UserDetails>),StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(int),  StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllUsersAsync()
        {
            var users =await _userManager.Users.ToListAsync();
            //return Ok(users);
            return Ok(_mapper.Map<IEnumerable<UserDetails>>(users));
        }

        [HttpGet("GetSearchedUsers")]
        //[Authorize(Roles = Roles.Admin, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<UserDetails>),StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(int),  StatusCodes.Status200OK)]
        public async Task<ActionResult> GetSearchedUsersAsync([FromQuery] string username)
        {
            var users =await _userManager.Users.Where(x=>x.UserName.Contains(username)).ToListAsync();
            //return Ok(users);
            return Ok(_mapper.Map<IEnumerable<UserDetails>>(users));
        }



        [HttpGet("GetUser/{username}")]
        //[Authorize(Roles = Roles.Admin + "," + Roles.Buyer,AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<UserDetails>),StatusCodes.Status200OK)]
        public async Task<ActionResult> GetUserAsync(string username)
        {
            var user =await  _userManager.Users.FirstOrDefaultAsync(user => user.UserName == username);
            //return Ok(user);
            return Ok(_mapper.Map<UserDetails>(user));
        
        }


    }
}

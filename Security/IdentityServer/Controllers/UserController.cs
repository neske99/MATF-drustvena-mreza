using AutoMapper;
using IdentityServer.Entities;
using IdentityService.DTOs;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserDetails>>(users));
        }

        [HttpGet("{username}")]
        [Authorize(Roles = "Admin,Buyer")]
        public async Task<ActionResult> GetUserAsync(string username)
        {
            var user =await  _userManager.Users.FirstOrDefaultAsync(user => user.UserName == username);

            return Ok(_mapper.Map<UserDetails>(user));
        
        }
    }
}

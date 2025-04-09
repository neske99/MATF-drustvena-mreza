using AutoMapper;
using EventBusMessages.Events;
using IdentityServer.Controllers;
using IdentityServer.Entities;
using IdentityService.DTOs;
using IdentityService.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers.Base
{
    public class RegistrationControllerBase : ControllerBase
    {
        protected readonly ILogger<AuthenticationController> _logger;
        protected readonly IMapper _mapper;
        protected readonly UserManager<User> _userManager;
        protected readonly RoleManager<IntRole> _roleManager;
        private readonly IPublishEndpoint _publishEndpoint;

        public RegistrationControllerBase(ILogger<AuthenticationController> logger, IMapper mapper, UserManager<User> userManager, RoleManager<IntRole> roleManager, IPublishEndpoint publishEndpoint)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        protected async Task<ActionResult> RegisterNewUserWithRoles(NewUserDto newUser, IEnumerable<string> roles)
        {
            var user = _mapper.Map<User>(newUser);
            var result = await _userManager.CreateAsync(user, newUser.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);

            }
            _logger.LogInformation("Successfully registered user");


            foreach (var role in roles)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (roleExists)
                {
                    await _userManager.AddToRoleAsync(user, role);
                    _logger.LogInformation("successfully added role to user user");
                }
                else
                {
                    _logger.LogInformation("role does not exist");

                }


            }

            var userCreatedEvent = new UserCreatedEvent() {UserId=user.Id, UserName = user.UserName, FirstName = user.FirstName, LastName = user.LastName };

            await _publishEndpoint.Publish(userCreatedEvent);
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}

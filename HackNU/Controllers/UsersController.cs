using System.Linq;
using System.Threading.Tasks;
using HackNU.Contracts.Requests;
using HackNU.Contracts.Responses;
using HackNU.Data;
using HackNU.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HackNU.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IUserService _userService;
        private readonly DataContext _context;
        public UsersController(IIdentityService identityService, IUserService userService, DataContext context)
        {
            this._identityService = identityService;
            _userService = userService;
            _context = context;
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("subscribe")]
        [SwaggerOperation(summary:"Subscribe user to event", description:"Gets user email and event id, subscribing user to event")]
        [SwaggerResponse(200, "Successful subscribing")]
        [SwaggerResponse(400, "Invalid parameters")]
        public async Task<IActionResult> Subscribe([FromBody] UserSubscribeToEventRequest request)
        {
            var result = await _userService.SubscribeAsync(request.Email, request.EventId);

            if (!result.Success)
            {
                return BadRequest(new InvalidParameterResponse());
            }

            return Ok(new SuccessResponse());
        }
        
        [HttpPost("login")]
        [SwaggerOperation(summary:"Log in by user email and password", description:"Log in by user email and password")]
        [SwaggerResponse(200, "Successful login")]
        [SwaggerResponse(400, "Invalid parameters")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("load")]
        [SwaggerOperation(summary:"Sent`s some user information", description:"Sent`s user email, nickname and subscribed events")]
        [SwaggerResponse(200, "Good request")]
        [SwaggerResponse(400, "Bad request")]
        public async Task<IActionResult> LoadUser([FromBody] UserLoadRequest request)
        {
            var loadUser = await _userService.LoadUserAsync(request.Email);

            if (loadUser == null)
            {
                return BadRequest("User with specified email not found");
            }

            return Ok(loadUser);
        }

        [HttpPost("register")]
        [SwaggerOperation(summary:"Registers new user", description:"Register a new user in a system")]
        [SwaggerResponse(200, "Registers new user")]
        [SwaggerResponse(400, "Invalid parameters")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }
            
            var authResponse = await _identityService.RegisterAsync(request.Email, request.Nickname, request.Password);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }
    }
}

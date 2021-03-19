using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackNU.Contracts.Requests;
using HackNU.Contracts.Responses;
using HackNU.Models;
using HackNU.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace HackNU.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public UsersController(IIdentityService identityService)
        {
            this._identityService = identityService;
        }

        [HttpPost("register")]
        [SwaggerOperation(summary:"Registers new user", description:"Register a new user in a system")]
        [SwaggerResponse(200, "Registers new user")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
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

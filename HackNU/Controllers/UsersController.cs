using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace HackNU.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        public UsersController()
        {
        }
        
        [HttpGet]
        [SwaggerOperation(summary:"Returns all users", description:"Returns all users")]
        [SwaggerResponse(200, "Returns all users")]
        public IActionResult Get()
        {
            return Ok(new{name = "Hello", age = 15});
        }
    }
}

using System.Threading.Tasks;
using HackNU.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HackNU.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("events")]
    public class EventsController : ControllerBase
    {
        public EventsController()
        {
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateEventContract eventContract)
        {
            return Ok();
        }
    }
}
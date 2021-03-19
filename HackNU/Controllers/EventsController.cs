using System.Threading.Tasks;
using HackNU.Contracts;
using HackNU.Data;
using HackNU.Models;
using HackNU.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HackNU.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("create")]
        [SwaggerOperation(summary:"Creates new event", description:"Creates new event with organizer email address")]
        [SwaggerResponse(200, "Creates event")]
        [SwaggerResponse(400, "Invalid parameters")]
        [SwaggerResponse(401, "No access. Token needed")]
        public async Task<IActionResult> Create([FromBody] CreateEventContract eventContract)
        {
            var createResult = await _eventService.Create(eventContract);
            if (!createResult.Success)
            {
                return BadRequest();
            }
            
            return Ok();
        }
    }
}
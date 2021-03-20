using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HackNU.Contracts;
using HackNU.Contracts.Requests;
using HackNU.Contracts.Responses;
using HackNU.Data;
using HackNU.Models;
using HackNU.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HackNU.Controllers
{
    [ApiController]
    [Route("events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }
        
        [HttpPost("get")]
        [SwaggerOperation(summary:"Return`s some events in specific order", description:"Return`s some events in specific order")]
        [SwaggerResponse(200, "Return`s events")]
        [SwaggerResponse(400, "Invalid parameters")]
        public async Task<IActionResult> Get([FromBody] GetEventsRequest request)
        {
            var events = await _eventService.FindAsync(request);

            // TODO: Add city validation
            return Ok(events);
        }
        
        [HttpPut("subscribe-to-tag")]
        [SwaggerOperation(summary:"Return`s some events in specific order", description:"Return`s some events in specific order")]
        [SwaggerResponse(200, "Return`s events")]
        [SwaggerResponse(400, "Invalid parameters")]
        public async Task<IActionResult> Subscribe(int eventId, int tagId)
        {
            var result = await _eventService.SubscribeAsync(eventId, tagId);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            
            return Ok(new SuccessResponse());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("create")]
        [SwaggerOperation(summary:"Creates new event", description:"Creates new event with organizer email address")]
        [SwaggerResponse(200, "Creates event")]
        [SwaggerResponse(400, "Invalid parameters")]
        [SwaggerResponse(401, "No access. Token needed")]
        public async Task<IActionResult> Create([FromBody] CreateEventContract eventContract)
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var createResult = await _eventService.CreateAsync(eventContract, email);
            
            if (!createResult.Success)
            {
                return BadRequest(new InvalidParameterResponse());
            }
            
            return Ok(new SuccessResponse());
        }
    }
}
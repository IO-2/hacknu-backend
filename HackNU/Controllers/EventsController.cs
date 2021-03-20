using System.Linq;
using System.Threading.Tasks;
using HackNU.Contracts;
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
        
        [HttpPost("get/search")]
        [SwaggerOperation(summary:"Return`s all events in city with query", description:"Returns all events in city with query")]
        [SwaggerResponse(200, "Return`s events")]
        [SwaggerResponse(400, "Invalid parameters")]
        public async Task<IActionResult> GetSearched(string city, string query)
        {
            var createResult = await _eventService.FindAsync(city);
            var result = _eventService.Query(createResult, query);
            
            // TODO: Add city validation
            return Ok(result);
        }
        
        [HttpPost("get/search/sorted-by-date")]
        [SwaggerOperation(summary:"Return`s all events in city sorted by date with query", description:"Return`s all events in city sorted by date with query")]
        [SwaggerResponse(200, "Return`s events")]
        [SwaggerResponse(400, "Invalid parameters")]
        public async Task<IActionResult> GetSearched(string city, string query, bool dateAscending)
        {
            var createResult = await _eventService.FindSortByDateAsync(city, dateAscending);
            var result = _eventService.Query(createResult, query);
            
            // TODO: Add city validation
            return Ok(result);
        }
        
        [HttpPost("get/search/sorted-by-location")]
        [SwaggerOperation(summary:"Return`s all events in city sorted by nearest with query", description:"Return`s all events in city sorted by nearest with query")]
        [SwaggerResponse(200, "Return`s events")]
        [SwaggerResponse(400, "Invalid parameters")]
        public async Task<IActionResult> GetSearched(string city, string query, float longitude, float latitude)
        {
            var createResult = await _eventService.FindNearestAsync(city, longitude, latitude);
            var result = _eventService.Query(createResult, query);
            
            // TODO: Add city validation
            return Ok(result);
        }
        
        [HttpGet("get")]
        [SwaggerOperation(summary:"Return`s all events in city", description:"Returns all events in city")]
        [SwaggerResponse(200, "Return`s events")]
        [SwaggerResponse(400, "Invalid parameters")]
        public async Task<IActionResult> Get(string city)
        {
            var createResult = await _eventService.FindAsync(city);
            // TODO: Add city validation
            return Ok(createResult);
        }
        
        [HttpGet("get/sorted-by-date")]
        [SwaggerOperation(summary:"Return`s all events in city sorted by date", description:"Return`s all events in city sorted by date")]
        [SwaggerResponse(200, "Return`s events")]
        [SwaggerResponse(400, "Invalid parameters")]
        public async Task<IActionResult> Get(string city, bool dateAscending)
        {
            var createResult = await _eventService.FindSortByDateAsync(city, dateAscending);
            // TODO: Add city validation
            return Ok(createResult);
        }
        
        [HttpGet("get/sorted-by-location")]
        [SwaggerOperation(summary:"Return`s all events in city sorted by nearest", description:"Return`s all events in city sorted by nearest")]
        [SwaggerResponse(200, "Return`s events")]
        [SwaggerResponse(400, "Invalid parameters")]
        public async Task<IActionResult> Get(string city, float longitude, float latitude)
        {
            var createResult = await _eventService.FindNearestAsync(city, longitude, latitude);
            // TODO: Add city validation
            return Ok(createResult);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("create")]
        [SwaggerOperation(summary:"Creates new event", description:"Creates new event with organizer email address")]
        [SwaggerResponse(200, "Creates event")]
        [SwaggerResponse(400, "Invalid parameters")]
        [SwaggerResponse(401, "No access. Token needed")]
        public async Task<IActionResult> Create([FromBody] CreateEventContract eventContract)
        {
            var createResult = await _eventService.CreateAsync(eventContract);
            if (!createResult.Success)
            {
                return BadRequest(new InvalidParameterResponse());
            }
            
            return Ok(new SuccessResponse());
        }
    }
}
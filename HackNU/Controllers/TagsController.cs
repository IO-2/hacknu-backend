using System.Security.Claims;
using System.Threading.Tasks;
using HackNU.Contracts.Responses;
using HackNU.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HackNU.Controllers
{
    [ApiController]
    [Route("tags")]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService _tagsService;
        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }
        
        [HttpGet("get")]
        [SwaggerOperation(summary:"Return`s all tags", description:"Return`s all tags")]
        [SwaggerResponse(200, "Return`s tags")]
        [SwaggerResponse(400, "Invalid parameters")]
        public async Task<IActionResult> Get()
        {
            var tags = _tagsService.GetTags();

            return Ok(tags);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("create")]
        [SwaggerOperation(summary:"Creates a new tag", description:"Creates a new tag")]
        [SwaggerResponse(200, "Return`s tags")]
        [SwaggerResponse(400, "Invalid parameters")]
        [SwaggerResponse(401, "Unauthorized")]
        public async Task<IActionResult> Create(string text)
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email != "art3a@niuitmo.ru")
            {
                return Unauthorized();
            }
            
            var createResult = await _tagsService.Create(text);

            if (!createResult.Success)
            {
                return BadRequest(createResult.Errors);
            }

            return Ok(new SuccessResponse());
        }
    }
}
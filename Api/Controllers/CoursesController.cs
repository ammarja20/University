using Api.Filters;
using Core.Forms;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [TypeFilter(typeof(ApiExceptionFilter))]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _service;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICourseService service, ILogger<CoursesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET /api/courses");
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GET /api/courses/{Id}", id);
            return Ok(await _service.GetByIdAsync(id)); // NotFound handled via filter
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCourseForm form)
        {
            _logger.LogInformation("POST /api/courses");
            var dto = await _service.CreateAsync(form);
            return Ok(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseForm form)
        {
            _logger.LogInformation("PUT /api/courses/{Id}", id);
            await _service.UpdateAsync(id, form);
            return Ok(new { Message = $"Course {id} updated successfully" });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE /api/courses/{Id}", id);
            await _service.DeleteAsync(id);
            return Ok(new { Message = $"Course {id} deleted successfully" });
        }
    }
}

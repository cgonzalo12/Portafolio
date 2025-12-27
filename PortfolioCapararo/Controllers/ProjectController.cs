using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioCapararo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IProjectService service, ILogger<ProjectController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los proyectos
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProjects([FromQuery] int? limit)
        {
            try
            {
                var result = await _service.GetAllProjectsAsync(limit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener proyectos");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectRequest request)
        {
            var result = await _service.CreateProjectAsync(request);
            return Created("", result);
        }
    }
}

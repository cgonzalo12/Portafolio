using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioCapararo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IEducationService _service;
        private readonly ILogger<EducationController> _logger;

        public EducationController(IEducationService service, ILogger<EducationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene toda la educación
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEducation()
        {
            try
            {
                var result = await _service.GetAllEducationAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener educación");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEducation([FromBody] CreateEducationRequest request)
        {
            var result = await _service.CreateEducationAsync(request);
            return Created("", result);
        }
    }
}

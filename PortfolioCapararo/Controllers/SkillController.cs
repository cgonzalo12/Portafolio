using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioCapararo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _service;
        private readonly ILogger<SkillController> _logger;

        public SkillController(ISkillService service, ILogger<SkillController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las habilidades
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSkills()
        {
            try
            {
                var result = await _service.GetAllSkillsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener habilidades");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene habilidades por categoría
        /// </summary>
        [HttpGet("category/{category}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSkillsByCategory(string category)
        {
            try
            {
                var result = await _service.GetSkillsByCategoryAsync(category);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener habilidades por categoría");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSkill(CreateSkillRequest request)
        {
            await _service.CreateSkillAsync(request);
            return Created("", new { message = "Skill creada" });
        }
    }
}

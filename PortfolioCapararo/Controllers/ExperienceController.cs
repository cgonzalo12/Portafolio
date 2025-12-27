using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioCapararo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly IExperienceService _service;
        private readonly ILogger<ExperienceController> _logger;

        public ExperienceController(IExperienceService service, ILogger<ExperienceController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las experiencias laborales
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllExperiences()
        {
            try
            {
                var result = await _service.GetAllExperiencesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener experiencias");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene una experiencia por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExperienceById(int id)
        {
            try
            {
                var result = await _service.GetExperienceByIdAsync(id);

                if (!result.Found)
                {
                    return NotFound(new { message = "Experiencia no encontrada" });
                }

                return Ok(result.Experience);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener experiencia {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea una nueva experiencia laboral
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateExperience([FromBody] CreateExperienceRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _service.CreateExperienceAsync(request);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return CreatedAtAction(
                    nameof(GetExperienceById),
                    new { id = result.Id },
                    result
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear experiencia");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza una experiencia existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateExperience(int id, [FromBody] UpdateExperienceRequest request)
        {
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new { message = "El ID de la URL no coincide con el ID del body" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _service.UpdateExperienceAsync(request);

                if (!result.Success)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar experiencia {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina una experiencia
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            try
            {
                var result = await _service.DeleteExperienceAsync(id);

                if (!result.Success)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar experiencia {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
}

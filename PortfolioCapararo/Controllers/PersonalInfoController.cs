using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioCapararo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalInfoController : ControllerBase
    {
        private readonly IPersonalInfoService _service;
        private readonly ILogger<PersonalInfoController> _logger;

        public PersonalInfoController(IPersonalInfoService service, ILogger<PersonalInfoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la información personal del portfolio
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPersonalInfo()
        {
            try
            {
                var result = await _service.GetPersonalInfoAsync();

                if (result == null)
                {
                    return NotFound(new { message = "Información personal no encontrada" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener información personal");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza la información personal del portfolio
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePersonalInfo([FromBody] UpdatePersonalInfoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _service.UpdatePersonalInfoAsync(request);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar información personal");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonalInfo(CreatePersonalInfoRequest request)
        {
            var result = await _service.CreatePersonalInfoAsync(request);
            if (!result.Success) return BadRequest(result);
            return Created("", result);
        }
    }
}

using HealthMed.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoEspecialidadeController : ControllerBase
    {
        private readonly IMedicoEspecialidadeService _service;

        public MedicoEspecialidadeController(IMedicoEspecialidadeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var especialidades = await _service.ObterTodasEspecialidades();
            return Ok(especialidades);
        }

    }
}

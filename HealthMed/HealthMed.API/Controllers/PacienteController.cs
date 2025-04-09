using HealthMed.Domain.DTO;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : HealthMedBaseController
    {
        private readonly IPacienteService _service;

        public PacienteController(IPacienteService service)
        {
            _service = service;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] PacienteLoginDTO loginRequest)
        {
            return TratarRetorno<string>(await _service.LoginAsync(loginRequest.CpfOrEmail, loginRequest.Senha));
        }

        [HttpPost("Cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] Paciente paciente)
        {
            return TratarRetorno<string>(await _service.CadastrarAsync(paciente));
        }
    }    
}

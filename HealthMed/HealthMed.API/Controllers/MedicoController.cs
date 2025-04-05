using HealthMed.Domain.DTO;
using HealthMed.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : HealthMedBaseController
    {
        private readonly IMedicoService _medicoService;

        public MedicoController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        [HttpPost("Cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] MedicoCadastroDTO medico)
        {
            return TratarRetorno<Guid?>(await _medicoService.CadastroMedico(medico));
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] MedicoLoginDTO loginRequest)
        {
            return TratarRetorno<MedicoLoginDTO?>(await _medicoService.Login(loginRequest));
        }
    }
}

using HealthMed.Domain.DTO;
using HealthMed.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Medico")]
    public class AgendaController : HealthMedBaseController
    {
        private readonly IAgendaService _service;

        public AgendaController(IAgendaService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return TratarRetorno(result);
        }

        [HttpGet("medico/{medicoId}")]
        public async Task<IActionResult> GetByMedicoId(Guid medicoId)
        {
            var result = await _service.GetByMedicoIdAsync(medicoId);
            return TratarRetorno(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(GerenciarAgendaDTO agenda)
        {
            var result = await _service.AddAsync(agenda);
            return TratarRetorno(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(GerenciarAgendaDTO agenda)
        {
            var result = await _service.UpdateAsync(agenda);
            return TratarRetorno(result);
        }
    }
}

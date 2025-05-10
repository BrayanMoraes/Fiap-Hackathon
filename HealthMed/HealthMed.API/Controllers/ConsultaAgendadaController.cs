using HealthMed.Domain.DTO;
using HealthMed.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Paciente, Medico")]
    public class ConsultaAgendadaController : HealthMedBaseController
    {
        private readonly IConsultaAgendadaService _service;

        public ConsultaAgendadaController(IConsultaAgendadaService service)
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

        [HttpGet("paciente/{pacienteId}")]
        public async Task<IActionResult> GetByPacienteId(Guid pacienteId)
        {
            var result = await _service.GetByPacienteIdAsync(pacienteId);
            return TratarRetorno(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ConsultaAgendadaDTO consultaAgendada)
        {
            var result = await _service.AddAsync(consultaAgendada);
            return TratarRetorno(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ConsultaAgendadaDTO consultaAgendada)
        {
            var result = await _service.UpdateAsync(consultaAgendada);
            return TratarRetorno(result);
        }

        [HttpPut("cancelar/{id}")]
        public async Task<IActionResult> CancelarConsulta(Guid id, [FromBody] string motivoCancelamento)
        {
            var result = await _service.CancelarConsultaAsync(id, motivoCancelamento);
            return TratarRetorno(result);
        }
    }
}

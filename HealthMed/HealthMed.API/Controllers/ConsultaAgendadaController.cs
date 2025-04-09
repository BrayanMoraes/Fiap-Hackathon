using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthMed.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Paciente")]
    public class ConsultaAgendadaController : ControllerBase
    {
        private readonly IConsultaAgendadaService _service;

        public ConsultaAgendadaController(IConsultaAgendadaService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var consulta = await _service.GetByIdAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }
            return Ok(consulta);
        }

        [HttpGet("medico/{medicoId}")]
        public async Task<IActionResult> GetByMedicoId(Guid medicoId)
        {
            var consultas = await _service.GetByMedicoIdAsync(medicoId);
            return Ok(consultas);
        }

        [HttpGet("paciente/{pacienteId}")]
        public async Task<IActionResult> GetByPacienteId(Guid pacienteId)
        {
            var consultas = await _service.GetByPacienteIdAsync(pacienteId);
            return Ok(consultas);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ConsultaAgendada consultaAgendada)
        {
            await _service.AddAsync(consultaAgendada);
            return CreatedAtAction(nameof(GetById), new { id = consultaAgendada.Id }, consultaAgendada);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ConsultaAgendada consultaAgendada)
        {
            if (id != consultaAgendada.Id)
            {
                return BadRequest();
            }

            await _service.UpdateAsync(consultaAgendada);
            return NoContent();
        }

        [HttpPut("cancelar/{id}")]
        public async Task<IActionResult> CancelarConsulta(Guid id, [FromBody] string motivoCancelamento)
        {
            await _service.CancelarConsultaAsync(id, motivoCancelamento);
            return NoContent();
        }
    }
}

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
    [Authorize(Roles = "Medico")]
    public class AgendaController : ControllerBase
    {
        private readonly IAgendaService _service;

        public AgendaController(IAgendaService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var agenda = await _service.GetByIdAsync(id);
            if (agenda == null)
            {
                return NotFound();
            }
            return Ok(agenda);
        }

        [HttpGet("medico/{medicoId}")]
        public async Task<IActionResult> GetByMedicoId(Guid medicoId)
        {
            var agendas = await _service.GetByMedicoIdAsync(medicoId);
            return Ok(agendas);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Agenda agenda)
        {
            await _service.AddAsync(agenda);
            return CreatedAtAction(nameof(GetById), new { id = agenda.Id }, agenda);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Agenda agenda)
        {
            if (id != agenda.Id)
            {
                return BadRequest();
            }

            await _service.UpdateAsync(agenda);
            return NoContent();
        }
    }
}

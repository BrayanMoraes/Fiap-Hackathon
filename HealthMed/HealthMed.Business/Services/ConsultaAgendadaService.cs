using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthMed.Business.Services
{
    public class ConsultaAgendadaService : IConsultaAgendadaService
    {
        private readonly IConsultaAgendadaRepository _repository;

        public ConsultaAgendadaService(IConsultaAgendadaRepository repository)
        {
            _repository = repository;
        }

        public async Task<ConsultaAgendada> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(ConsultaAgendada consultaAgendada)
        {
            await _repository.AddAsync(consultaAgendada);
        }

        public async Task UpdateAsync(ConsultaAgendada consultaAgendada)
        {
            await _repository.UpdateAsync(consultaAgendada);
        }

        public async Task<IEnumerable<ConsultaAgendada>> GetByMedicoIdAsync(Guid medicoId)
        {
            return await _repository.GetByMedicoIdAsync(medicoId);
        }

        public async Task<IEnumerable<ConsultaAgendada>> GetByPacienteIdAsync(Guid pacienteId)
        {
            return await _repository.GetByPacienteIdAsync(pacienteId);
        }

        public async Task CancelarConsultaAsync(Guid id, string motivoCancelamento)
        {
            var consulta = await _repository.GetByIdAsync(id);
            if (consulta != null)
            {
                consulta.Cancelada = true;
                consulta.MotivoCancelamento = motivoCancelamento;
                await _repository.UpdateAsync(consulta);
            }
        }
    }
}

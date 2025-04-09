using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthMed.Business.Services
{
    public class AgendaService : IAgendaService
    {
        private readonly IAgendaRepository _repository;

        public AgendaService(IAgendaRepository repository)
        {
            _repository = repository;
        }

        public async Task<Agenda> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Agenda agenda)
        {
            await _repository.AddAsync(agenda);
        }

        public async Task UpdateAsync(Agenda agenda)
        {
            await _repository.UpdateAsync(agenda);
        }

        public async Task<IEnumerable<Agenda>> GetByMedicoIdAsync(Guid medicoId)
        {
            return await _repository.GetByMedicoIdAsync(medicoId);
        }
    }
}

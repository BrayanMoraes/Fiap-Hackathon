using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Shared;
using HealthMed.Domain.Enum;
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

        public async Task<OperationResult<Agenda>> GetByIdAsync(Guid id)
        {
            var agenda = await _repository.GetByIdAsync(id);
            if (agenda == null)
            {
                return new OperationResult<Agenda>
                {
                    Status = TypeReturnStatus.Error,
                    Message = "Agenda não encontrada."
                };
            }

            return new OperationResult<Agenda>
            {
                Status = TypeReturnStatus.Success,
                ResultObject = agenda
            };
        }

        public async Task<OperationResult<object>> AddAsync(Agenda agenda)
        {
            await _repository.AddAsync(agenda);
            return new OperationResult<object>
            {
                Status = TypeReturnStatus.Success,
                Message = "Agenda adicionada com sucesso."
            };
        }

        public async Task<OperationResult<object>> UpdateAsync(Agenda agenda)
        {
            await _repository.UpdateAsync(agenda);
            return new OperationResult<object>
            {
                Status = TypeReturnStatus.Success,
                Message = "Agenda atualizada com sucesso."
            };
        }

        public async Task<OperationResult<IEnumerable<Agenda>>> GetByMedicoIdAsync(Guid medicoId)
        {
            var agendas = await _repository.GetByMedicoIdAsync(medicoId);
            return new OperationResult<IEnumerable<Agenda>>
            {
                Status = TypeReturnStatus.Success,
                ResultObject = agendas
            };
        }
    }
}

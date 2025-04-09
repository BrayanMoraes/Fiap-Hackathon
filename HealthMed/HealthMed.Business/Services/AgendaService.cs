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
            try
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
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<Agenda>(ex, "Erro ao buscar agenda.");
            }
        }

        public async Task<OperationResult<object>> AddAsync(Agenda agenda)
        {
            try
            {
                await _repository.AddAsync(agenda);
                return new OperationResult<object>
                {
                    Status = TypeReturnStatus.Success,
                    Message = "Agenda adicionada com sucesso."
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<object>(ex, "Erro ao adicionar agenda.");
            }
        }

        public async Task<OperationResult<object>> UpdateAsync(Agenda agenda)
        {
            try
            {
                await _repository.UpdateAsync(agenda);
                return new OperationResult<object>
                {
                    Status = TypeReturnStatus.Success,
                    Message = "Agenda atualizada com sucesso."
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<object>(ex, "Erro ao atualizar agenda.");
            }
        }

        public async Task<OperationResult<IEnumerable<Agenda>>> GetByMedicoIdAsync(Guid medicoId)
        {
            try
            {
                var agendas = await _repository.GetByMedicoIdAsync(medicoId);
                return new OperationResult<IEnumerable<Agenda>>
                {
                    Status = TypeReturnStatus.Success,
                    ResultObject = agendas
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<IEnumerable<Agenda>>(ex, "Erro ao buscar agendas por médico.");
            }
        }
    }
}

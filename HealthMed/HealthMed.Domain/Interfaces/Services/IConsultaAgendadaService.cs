using HealthMed.Domain.DTO;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Shared;

namespace HealthMed.Domain.Interfaces.Services
{
    public interface IConsultaAgendadaService
    {
        Task<OperationResult<ConsultaAgendada>> GetByIdAsync(Guid id);
        Task<OperationResult<object>> AddAsync(ConsultaAgendadaDTO consultaAgendada);
        Task<OperationResult<object>> UpdateAsync(ConsultaAgendadaDTO consultaAgendada);
        Task<OperationResult<IEnumerable<ConsultaAgendada>>> GetByMedicoIdAsync(Guid medicoId);
        Task<OperationResult<IEnumerable<ConsultaAgendada>>> GetByPacienteIdAsync(Guid pacienteId);
        Task<OperationResult<object>> CancelarConsultaAsync(Guid id, string motivoCancelamento);
    }
}


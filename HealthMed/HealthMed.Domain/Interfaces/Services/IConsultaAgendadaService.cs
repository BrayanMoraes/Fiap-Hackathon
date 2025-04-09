using HealthMed.Domain.Entities;
using HealthMed.Domain.Shared;

namespace HealthMed.Domain.Interfaces.Services
{
    public interface IConsultaAgendadaService
    {
        Task<OperationResult<ConsultaAgendada>> GetByIdAsync(Guid id);
        Task<OperationResult<object>> AddAsync(ConsultaAgendada consultaAgendada);
        Task<OperationResult<object>> UpdateAsync(ConsultaAgendada consultaAgendada);
        Task<OperationResult<IEnumerable<ConsultaAgendada>>> GetByMedicoIdAsync(Guid medicoId);
        Task<OperationResult<IEnumerable<ConsultaAgendada>>> GetByPacienteIdAsync(Guid pacienteId);
        Task<OperationResult<object>> CancelarConsultaAsync(Guid id, string motivoCancelamento);
    }
}


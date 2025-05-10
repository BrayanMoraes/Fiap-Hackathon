using HealthMed.Domain.DTO;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Shared;

namespace HealthMed.Domain.Interfaces.Services
{
    public interface IAgendaService
    {
        Task<OperationResult<AgendaRetornoDTO>> GetByIdAsync(Guid id);
        Task<OperationResult<object>> AddAsync(GerenciarAgendaDTO agenda);
        Task<OperationResult<object>> UpdateAsync(GerenciarAgendaDTO agenda);
        Task<OperationResult<IEnumerable<AgendaRetornoDTO>>> GetByMedicoIdAsync(Guid medicoId);
    }
}


using HealthMed.Domain.Entities;
using HealthMed.Domain.Shared;

namespace HealthMed.Domain.Interfaces.Services
{
    public interface IAgendaService
    {
        Task<OperationResult<Agenda>> GetByIdAsync(Guid id);
        Task<OperationResult<object>> AddAsync(Agenda agenda);
        Task<OperationResult<object>> UpdateAsync(Agenda agenda);
        Task<OperationResult<IEnumerable<Agenda>>> GetByMedicoIdAsync(Guid medicoId);
    }
}


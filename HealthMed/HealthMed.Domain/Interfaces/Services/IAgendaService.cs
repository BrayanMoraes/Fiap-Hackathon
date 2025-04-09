using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Services
{
    public interface IAgendaService
    {
        Task<Agenda> GetByIdAsync(Guid id);
        Task AddAsync(Agenda agenda);
        Task UpdateAsync(Agenda agenda);
        Task<IEnumerable<Agenda>> GetByMedicoIdAsync(Guid medicoId);
    }
}

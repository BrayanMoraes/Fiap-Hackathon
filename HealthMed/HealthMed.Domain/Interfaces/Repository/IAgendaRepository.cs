using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Repository
{
    public interface IAgendaRepository
    {
        Task<Agenda> GetByIdAsync(Guid id);
        Task AddAsync(Agenda agenda);
        Task UpdateAsync(Agenda agenda);
        Task<IEnumerable<Agenda>> GetByMedicoIdAsync(Guid medicoId);
    }
}

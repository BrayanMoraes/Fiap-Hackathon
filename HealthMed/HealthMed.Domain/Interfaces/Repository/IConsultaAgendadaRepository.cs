using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Repository
{
    public interface IConsultaAgendadaRepository
    {
        Task<ConsultaAgendada> GetByIdAsync(Guid id);
        Task AddAsync(ConsultaAgendada consultaAgendada);
        Task UpdateAsync(ConsultaAgendada consultaAgendada);
        Task<IEnumerable<ConsultaAgendada>> GetByMedicoIdAsync(Guid medicoId);
        Task<IEnumerable<ConsultaAgendada>> GetByPacienteIdAsync(Guid pacienteId);
    }
}

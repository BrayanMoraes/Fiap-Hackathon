using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Repository
{
    public interface IMedicoRepository
    {
        Task<IEnumerable<Guid>> GetByEspecialidadeAsync(Guid especialidade);
        Task<Medico> GetByCRMAsync(string crm);
        Task AddAsync(Medico medico);

    }
}

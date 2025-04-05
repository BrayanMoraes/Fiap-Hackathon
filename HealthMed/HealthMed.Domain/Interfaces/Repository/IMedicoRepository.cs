using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Repository
{
    public interface IMedicoRepository
    {
        Task<Medico> GetByCRMAsync(string crm);
        Task AddAsync(Medico medico);

    }
}

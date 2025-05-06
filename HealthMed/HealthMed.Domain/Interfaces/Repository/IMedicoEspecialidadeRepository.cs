using HealthMed.Domain.DTO;
using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Repository
{
    public interface IMedicoEspecialidadeRepository
    {
        Task<ICollection<MedicoEspecialidadeDTO>> ObterTodasEspecialidades();
        Task AddAsync(MedicoEspecialidade especialidade);
    }
}

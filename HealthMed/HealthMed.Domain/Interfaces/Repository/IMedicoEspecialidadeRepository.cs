using HealthMed.Domain.DTO;

namespace HealthMed.Domain.Interfaces.Repository
{
    public interface IMedicoEspecialidadeRepository
    {
        Task<ICollection<MedicoEspecialidadeDTO>> ObterTodasEspecialidades();
    }
}

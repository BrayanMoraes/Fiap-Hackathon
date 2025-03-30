using HealthMed.Domain.DTO;

namespace HealthMed.Domain.Interfaces.Services
{
    public interface IMedicoEspecialidadeService
    {
        Task<ICollection<MedicoEspecialidadeDTO>> ObterTodasEspecialidades();
    }
}

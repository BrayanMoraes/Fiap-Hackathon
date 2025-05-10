using HealthMed.Domain.DTO;
using HealthMed.Domain.Shared;

namespace HealthMed.Domain.Interfaces.Services
{
    public interface IMedicoEspecialidadeService
    {
        Task<OperationResult<ICollection<MedicoEspecialidadeDTO>>> ObterTodasEspecialidades();
    }
}

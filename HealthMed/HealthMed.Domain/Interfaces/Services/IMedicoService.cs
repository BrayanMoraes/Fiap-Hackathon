using HealthMed.Domain.DTO;
using HealthMed.Domain.Shared;

namespace HealthMed.Domain.Interfaces.Services
{
    public interface IMedicoService
    {
        Task<OperationResult<Guid?>> CadastroMedico(MedicoCadastroDTO medicoCadastroDTO);
        Task<OperationResult<MedicoLoginRetornoDTO?>> Login(MedicoLoginDTO medicoLoginDTO);
    }
}

using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Repository
{
    public interface IPacienteRepository
    {
        Task<Paciente?> GetByCpfOrEmailAsync(string cpf, string email);
        Task AddAsync(Paciente paciente);
    }
}

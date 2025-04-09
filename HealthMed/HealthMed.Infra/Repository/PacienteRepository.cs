using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Infra.Repository
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly AppDbContext _context;

        public PacienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Paciente paciente)
        {
            await _context.Pacientes.AddAsync(paciente);
            await _context.SaveChangesAsync();

        }

        public async Task<Paciente?> GetByCpfOrEmailAsync(string cpf, string email)
        {
            return await _context.Pacientes
            .FirstOrDefaultAsync(p => p.CPF == cpf || p.Email == email);

        }
    }
}

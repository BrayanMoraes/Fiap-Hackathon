using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Infra.Repository
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly AppDbContext _context;

        public MedicoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Medico> GetByCRMAsync(string crm)
        {
            return await _context.Medicos.FirstOrDefaultAsync(m => m.CRM == crm);
        }

        public async Task AddAsync(Medico medico)
        {
            await _context.Medicos.AddAsync(medico);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Guid>> GetByEspecialidadeAsync(Guid especialidade)
        {
            return await _context.Medicos.Where(m => m.IdEspecialidade == especialidade)
                .Select(m => m.Id)
                .ToListAsync();
        }
    }
}

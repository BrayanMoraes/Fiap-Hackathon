using HealthMed.Domain.DTO;
using HealthMed.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Infra.Repository
{
    internal class MedicoEspecialidadeRepository : IMedicoEspecialidadeRepository
    {
        private readonly DbContext _context;

        public MedicoEspecialidadeRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<MedicoEspecialidadeDTO>> ObterTodasEspecialidades()
        {
            return await _context.Set<MedicoEspecialidadeDTO>().ToListAsync();
        }

    }
}

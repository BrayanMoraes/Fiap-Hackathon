using HealthMed.Domain.DTO;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Infra.Repository
{
    public class MedicoEspecialidadeRepository : IMedicoEspecialidadeRepository
    {
        private readonly AppDbContext _context;

        public MedicoEspecialidadeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<MedicoEspecialidadeDTO>> ObterTodasEspecialidades()
        {
            return await _context.MedicoEspecialidades.Select(x => new MedicoEspecialidadeDTO
            {
                IdEspecialidade = x.Id,
                DescricaoEspecialidade = x.Descricao
            }).ToListAsync();
        }

        public async Task AddAsync(MedicoEspecialidade especialidade)
        {
            await _context.MedicoEspecialidades.AddAsync(especialidade);
            await _context.SaveChangesAsync();
        }
    }
}

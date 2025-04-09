using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthMed.Infra.Repositories
{
    public class AgendaRepository : IAgendaRepository
    {
        private readonly AppDbContext _context;

        public AgendaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Agenda> GetByIdAsync(Guid id)
        {
            return await _context.Agendas.FindAsync(id);
        }

        public async Task AddAsync(Agenda agenda)
        {
            await _context.Agendas.AddAsync(agenda);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Agenda agenda)
        {
            _context.Agendas.Update(agenda);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Agenda>> GetByMedicoIdAsync(Guid medicoId)
        {
            return await _context.Agendas.Where(a => a.IdMedico == medicoId).ToListAsync();
        }
    }
}

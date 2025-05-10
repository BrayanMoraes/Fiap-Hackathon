using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthMed.Infra.Repositories
{
    public class ConsultaAgendadaRepository : IConsultaAgendadaRepository
    {
        private readonly AppDbContext _context;

        public ConsultaAgendadaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ConsultaAgendada> GetByIdAsync(Guid id)
        {
            return await _context.ConsultasAgendadas.FindAsync(id);
        }

        public async Task AddAsync(ConsultaAgendada consultaAgendada)
        {
            await _context.ConsultasAgendadas.AddAsync(consultaAgendada);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ConsultaAgendada consultaAgendada)
        {
            _context.ConsultasAgendadas.Update(consultaAgendada);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ConsultaAgendada>> GetByMedicoIdAsync(Guid medicoId)
        {
            return await _context.ConsultasAgendadas.Where(c => c.IdMedico == medicoId).ToListAsync();
        }

        public async Task<IEnumerable<ConsultaAgendada>> GetByPacienteIdAsync(Guid pacienteId)
        {
            return await _context.ConsultasAgendadas.Where(c => c.IdPaciente == pacienteId).ToListAsync();
        }
    }
}

using HealthMed.Domain.Entities;
using HealthMed.Infra.Configuration.EntitiesTypeConfiguration;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Agenda> Agendas { get; set; }
        public DbSet<ConsultaAgendada> ConsultasAgendadas { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<MedicoEspecialidade> MedicoEspecialidades { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Agenda>();
            modelBuilder.Entity<ConsultaAgendada>();
            modelBuilder.Entity<Medico>();
            modelBuilder.Entity<MedicoEspecialidade>();
            modelBuilder.Entity<Paciente>();

            modelBuilder.ApplyConfiguration(new AgendaConfiguration());
            modelBuilder.ApplyConfiguration(new ConsultaAgendadaConfiguration());
            modelBuilder.ApplyConfiguration(new MedicoConfiguration());
            modelBuilder.ApplyConfiguration(new MedicoEspecialidadeConfiguration());
            modelBuilder.ApplyConfiguration(new PacienteConfiguration());
        }
    }
}

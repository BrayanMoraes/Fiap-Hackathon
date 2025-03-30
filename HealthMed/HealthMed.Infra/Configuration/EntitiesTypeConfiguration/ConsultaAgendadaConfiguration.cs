using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Infra.Configuration.EntitiesTypeConfiguration
{
    internal class ConsultaAgendadaConfiguration : IEntityTypeConfiguration<ConsultaAgendada>
    {
        public void Configure(EntityTypeBuilder<ConsultaAgendada> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.IdPaciente).HasColumnName("ID_PACIENTE").IsRequired();
            builder.Property(x => x.IdMedico).HasColumnName("ID_MEDICO").IsRequired();
            builder.Property(x => x.IdAgenda).HasColumnName("ID_AGENDA").IsRequired();
            builder.Property(x => x.Aprovado).HasColumnName("APROVADO");
            builder.Property(x => x.Cancelada).HasColumnName("CANCELADA");
            builder.Property(x => x.MotivoCancelamento).HasColumnName("MOTIVO_CANCELAMENTO");
            builder.HasOne(x => x.Agenda).WithMany(x => x.ConsultasAgendadas);
            builder.HasOne(x => x.Medico).WithMany(x => x.ConsultasAgendadas);
            builder.HasOne(x => x.Paciente).WithMany(x => x.ConsultasAgendadas);
        }
    }
}

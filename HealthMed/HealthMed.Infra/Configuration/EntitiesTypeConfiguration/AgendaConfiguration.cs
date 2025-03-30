using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Infra.Configuration.EntitiesTypeConfiguration
{
    internal class AgendaConfiguration : IEntityTypeConfiguration<Agenda>
    {
        public void Configure(EntityTypeBuilder<Agenda> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.IdMedico).HasColumnName("ID_MEDICO").IsRequired();
            builder.Property(x => x.Horario).HasColumnName("HORARIO").IsRequired();
            builder.Property(x => x.Data).HasColumnName("DATA").IsRequired();
            builder.Property(x => x.ValorConsulta).HasColumnName("VALOR_CONSULTA").IsRequired().HasColumnType("DECIMAL(18,2)");
            builder.HasOne(x => x.Medico).WithMany(x => x.Agendas).HasForeignKey(x => x.IdMedico);
        }
    }
}

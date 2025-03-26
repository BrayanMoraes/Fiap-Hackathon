using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Infra.Configuration.EntitiesTypeConfiguration
{
    internal class AgendaConfiguration : IEntityTypeConfiguration<Agenda>
    {
        public void Configure(EntityTypeBuilder<Agenda> builder)
        {
            builder.HasKey(x => x.IdAgenda);
            builder.Property(x => x.IdMedico).IsRequired();
            builder.Property(x => x.Horario).IsRequired();
            builder.Property(x => x.Data).IsRequired();
            builder.Property(x => x.ValorConsulta).IsRequired().HasColumnType("DECIMAL(18,2)");
            builder.HasOne(x => x.Medico).WithMany(x => x.Agendas).HasForeignKey(x => x.IdMedico);
        }
    }
}

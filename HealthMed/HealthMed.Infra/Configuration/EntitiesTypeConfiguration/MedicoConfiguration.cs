using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Infra.Configuration.EntitiesTypeConfiguration
{
    internal class MedicoConfiguration : IEntityTypeConfiguration<Medico>
    {
        public void Configure(EntityTypeBuilder<Medico> builder)
        {
            builder.ToTable("TB_MEDICO");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.CRM).HasColumnName("CRM").IsRequired();
            builder.Property(x => x.Nome).HasColumnName("NOME").IsRequired();
            builder.Property(x => x.IdEspecialidade).HasColumnName("ID_ESPECIALIDADE").IsRequired();
            builder.Property(x => x.Senha).HasColumnName("SENHA").IsRequired();
            builder.HasMany(x => x.Agendas).WithOne(x => x.Medico);
            builder.HasMany(x => x.ConsultasAgendadas).WithOne(x => x.Medico).HasForeignKey(x => x.IdMedico);
            builder.HasOne(x => x.MedicoEspecialidade).WithMany(x => x.Medicos).HasForeignKey(x => x.IdEspecialidade);
        }
    }
}

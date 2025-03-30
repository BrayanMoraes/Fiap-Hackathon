using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Infra.Configuration.EntitiesTypeConfiguration
{
    internal class PacienteConfiguration : IEntityTypeConfiguration<Paciente>
    {
        public void Configure(EntityTypeBuilder<Paciente> builder)
        {
            builder.ToTable("TB_PACIENTE");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID").IsRequired();
            builder.Property(x => x.Senha).HasColumnName("SENHA").IsRequired();
            builder.Property(x => x.Email).HasColumnName("EMAIL").IsRequired();
            builder.Property(x => x.CPF).HasColumnName("CPF").IsRequired();
            builder.HasMany(x => x.ConsultasAgendadas).WithOne(x => x.Paciente).HasForeignKey(x => x.IdPaciente);
        }
    }
}

using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Infra.Configuration.EntitiesTypeConfiguration
{
    internal class MedicoEspecialidadeConfiguration : IEntityTypeConfiguration<MedicoEspecialidade>
    {
        public void Configure(EntityTypeBuilder<MedicoEspecialidade> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID").IsRequired();
            builder.Property(x => x.Descricao).HasColumnName("DESCRICAO").IsRequired();
            builder.HasMany(x => x.Medicos).WithOne(x => x.MedicoEspecialidade);
        }
    }
}

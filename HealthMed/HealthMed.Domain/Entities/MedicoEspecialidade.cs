namespace HealthMed.Domain.Entities
{
    public class MedicoEspecialidade
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public virtual ICollection<Medico> Medicos { get; set; }
    }
}

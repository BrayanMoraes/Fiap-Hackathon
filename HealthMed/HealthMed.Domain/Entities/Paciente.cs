namespace HealthMed.Domain.Entities
{
    public class Paciente
    {
        public Guid Id { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public virtual ICollection<ConsultasAgendadas> ConsultasAgendadas { get; set; }
    }
}

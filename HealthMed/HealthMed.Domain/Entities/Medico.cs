namespace HealthMed.Domain.Entities
{
    public class Medico
    {
        public Guid Id { get; set; }
        public string CRM { get; set; }
        public string Nome { get; set; }
        public Guid IdEspecialidade { get; set; }
        public string Senha { get; set; }
        public virtual ICollection<Agenda> Agendas { get; set; }
        public virtual ICollection<ConsultasAgendadas> ConsultasAgendadas { get; set; }
        public virtual MedicoEspecialidade MedicoEspecialidade { get; set; }
    }
}

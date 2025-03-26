namespace HealthMed.Domain.Entities
{
    public class Agenda
    {
        public Guid IdAgenda { get; set; }
        public string CRM { get; set; }
        public TimeSpan Horario { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorConsulta { get; set; }
        public virtual Medico Medico { get; set; }
        public virtual ICollection<ConsultasAgendadas> ConsultasAgendadas { get; set; }
    }
}

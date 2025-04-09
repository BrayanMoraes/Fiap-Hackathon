namespace HealthMed.Domain.Entities
{
    public class Agenda
    {
        public Guid Id { get; set; }
        public Guid IdMedico { get; set; }
        public TimeSpan Horario { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorConsulta { get; set; }
        public virtual Medico Medico { get; set; }
        public virtual ICollection<ConsultaAgendada> ConsultasAgendadas { get; set; }

        public bool ValidarDados()
        {
            if (Horario < TimeSpan.Zero || Horario >= TimeSpan.FromHours(24))
                return false;

            if (Data < DateTime.Today)
                return false;

            if (ValorConsulta <= 0)
                return false;

            return true;
        }
    }
}

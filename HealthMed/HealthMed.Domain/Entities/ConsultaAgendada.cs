namespace HealthMed.Domain.Entities
{
    public class ConsultaAgendada
    {
        public Guid Id { get; set; }
        public Guid IdPaciente { get; set; }
        public Guid IdMedico { get; set; }
        public Guid IdAgenda { get; set; }
        public bool? Aprovado { get; set; }
        public bool? Cancelada { get; set; }
        public string? MotivoCancelamento { get; set; }
        public virtual Paciente Paciente { get; set; }
        public virtual Medico Medico { get; set; }
        public virtual Agenda Agenda { get; set; }
    }
}

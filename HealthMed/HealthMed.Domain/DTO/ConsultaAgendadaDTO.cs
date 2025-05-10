using HealthMed.Domain.Enum;

namespace HealthMed.Domain.DTO
{
    public class ConsultaAgendadaDTO
    {
        public Guid Id { get; set; }
        public Guid IdPaciente { get; set; }
        public Guid IdMedico { get; set; }
        public Guid IdAgenda { get; set; }
        public bool? Aprovado { get; set; }
        public bool Cancelada { get; set; }
        public string? MotivoCancelamento { get; set; }
        public Solicitante Solicitante { get; set; }
    }
}

namespace HealthMed.Domain.DTO
{
    public class AgendaRetornoDTO
    {
        public Guid Id { get; set; }
        public Guid IdMedico { get; set; }
        public TimeSpan Horario { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorConsulta { get; set; }
    }
}

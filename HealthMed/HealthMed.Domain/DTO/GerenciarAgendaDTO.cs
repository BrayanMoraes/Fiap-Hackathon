namespace HealthMed.Domain.DTO
{
    public class GerenciarAgendaDTO
    {
        public Guid Id { get; set; }
        public Guid IdMedico { get; set; }
        public TimeSpan Horario { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorConsulta { get; set; }

        public bool IsHorarioValido()
        {
            return Horario >= TimeSpan.FromHours(8) && Horario <= TimeSpan.FromHours(18);
        }

        public bool IsDataValida()
        {
            return Data.Date >= DateTime.Now.Date;
        }

        public bool IsValorConsultaValido()
        {
            return ValorConsulta > 0;
        }

        public bool IsAgendaValida()
        {
            return IsHorarioValido() && IsDataValida() && IsValorConsultaValido();
        }
    }
}

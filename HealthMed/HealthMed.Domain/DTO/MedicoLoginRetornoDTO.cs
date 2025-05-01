namespace HealthMed.Domain.DTO
{
    public class MedicoLoginRetornoDTO
    {
        public Guid MedicoId { get; set; }
        public string NomeMedico { get; set; }
        public string Token { get; set; }
    }
}

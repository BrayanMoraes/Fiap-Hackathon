namespace HealthMed.Domain.DTO
{
    public class MedicoLoginDTO
    {
        public Guid MedicoId { get; set; }
        public string NomeMedico { get; set; }
        public string Token { get; set; }
        public string CRM { get; set; }
        public string Senha { get; set; }
    }
}

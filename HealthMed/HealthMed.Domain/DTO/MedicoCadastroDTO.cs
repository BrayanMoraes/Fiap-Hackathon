namespace HealthMed.Domain.DTO
{
    public class MedicoCadastroDTO
    {
        public string CRM { get; set; }
        public string NomeCompleto { get; set; }
        public Guid IdEspecialidade { get; set; }
        public string Senha { get; set; }
    }
}

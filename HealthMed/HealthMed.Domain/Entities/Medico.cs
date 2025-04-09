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
        public virtual ICollection<ConsultaAgendada> ConsultasAgendadas { get; set; }
        public virtual MedicoEspecialidade MedicoEspecialidade { get; set; }

        public bool ValidarDados()
        {
            return ValidarCRM() && ValidarNome() && ValidarSenha();
        }

        private bool ValidarCRM()
        {
            return !string.IsNullOrWhiteSpace(CRM) && CRM.Length == 6; // Example validation
        }

        private bool ValidarNome()
        {
            return !string.IsNullOrWhiteSpace(Nome) && Nome.Length >= 3;
        }

        private bool ValidarSenha()
        {
            return !string.IsNullOrWhiteSpace(Senha) && Senha.Length >= 6;
        }
    }
}

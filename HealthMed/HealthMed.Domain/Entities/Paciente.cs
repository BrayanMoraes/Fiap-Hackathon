using System.Text.RegularExpressions;

namespace HealthMed.Domain.Entities
{
    public class Paciente
    {
        public Guid Id { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public virtual ICollection<ConsultaAgendada> ConsultasAgendadas { get; set; }
        public bool ValidarDados()
        {
            return IsValidCPF() && IsValidEmail();
        }
        private bool IsValidEmail()
        {
            if (string.IsNullOrWhiteSpace(Email))
                return false;

            try
            {
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(Email, emailPattern);
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidCPF()
        {
            if (string.IsNullOrWhiteSpace(CPF))
                return false;

            var cpf = CPF.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11 || !long.TryParse(cpf, out _))
                return false;

            var tempCpf = cpf.Substring(0, 9);
            var sum = 0;

            for (var i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * (10 - i);

            var remainder = sum % 11;
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;

            var digit = remainder.ToString();
            tempCpf += digit;
            sum = 0;

            for (var i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * (11 - i);

            remainder = sum % 11;
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;

            digit += remainder.ToString();

            return cpf.EndsWith(digit);
        }
    }
}

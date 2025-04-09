using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Shared;

namespace HealthMed.Business.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;
        private readonly string _jwtSecret = "sua-chave-secreta-muito-segura"; // Substitua por uma chave segura

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<string>> LoginAsync(string cpfOrEmail, string senha)
        {
            var paciente = await _repository.GetByCpfOrEmailAsync(cpfOrEmail, cpfOrEmail);
            if (paciente == null || !BCrypt.Net.BCrypt.Verify(senha, paciente.Senha))
            {
                return null; // Retorna null caso não seja autenticado
            }

            var token = GerarTokenJWT(paciente); // Gera o token JWT

            return new OperationResult<string>
            {
                Status = Domain.Enum.TypeReturnStatus.Success,
                Message = "Login realizado com sucesso.",
                ResultObject = token
            };
        }

        public async Task<OperationResult<string>> CadastrarAsync(Paciente paciente)
        {
            var pacienteExistente = await _repository.GetByCpfOrEmailAsync(paciente.CPF, paciente.Email);
            if (pacienteExistente != null)
            {
                return new OperationResult<string>
                {
                    Status = Domain.Enum.TypeReturnStatus.Conflict,
                    Message = "Paciente com este CPF ou e-mail já está cadastrado."
                };
            }

            paciente.Senha = BCrypt.Net.BCrypt.HashPassword(paciente.Senha);
            paciente.Id = Guid.NewGuid();

            await _repository.AddAsync(paciente);

            return new OperationResult<string>
            {
                Status = Domain.Enum.TypeReturnStatus.Success,
                Message = "Paciente cadastrado com sucesso."
            };
        }

        private string GerarTokenJWT(Paciente paciente)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, paciente.Id.ToString()),
                new Claim(ClaimTypes.Email, paciente.Email),
                new Claim("CPF", paciente.CPF),
                new Claim(ClaimTypes.Role, "Paciente")
            }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

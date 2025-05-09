using HealthMed.Domain.DTO;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Enum;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Shared;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthMed.Business.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;
        private readonly string _jwtSecret = "D43493B1-FD3A-49DB-83FF-531A61A5313A";

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<string>> LoginAsync(string cpfOrEmail, string senha)
        {
            try
            {

                var paciente = await _repository.GetByCpfOrEmailAsync(cpfOrEmail, cpfOrEmail);
                if (paciente == null || !BCrypt.Net.BCrypt.Verify(senha, paciente.Senha))
                {
                    return new OperationResult<string>
                    {
                        Status = TypeReturnStatus.Error,
                        Message = "CPF ou senha inválidos."
                    };
                }

                var token = GerarTokenJWT(paciente);

                return new OperationResult<string>
                {
                    Status = TypeReturnStatus.Success,
                    Message = "Login realizado com sucesso.",
                    ResultObject = token
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<string>(ex, "Erro ao realizar login.");
            }
        }

        public async Task<OperationResult<string>> CadastrarAsync(PacienteCadastroDTO paciente)
        {
            try
            {
                var pacienteExistente = await _repository.GetByCpfOrEmailAsync(paciente.Cpf, paciente.Email);
                if (pacienteExistente != null)
                {
                    return new OperationResult<string>
                    {
                        Status = TypeReturnStatus.Conflict,
                        Message = "Paciente com este CPF ou e-mail já está cadastrado."
                    };
                }

                var pacienteNovo = new Paciente
                {
                    CPF = paciente.Cpf,
                    Email = paciente.Email,
                    Senha = BCrypt.Net.BCrypt.HashPassword(paciente.Senha),
                    Id = Guid.NewGuid()
                };

                await _repository.AddAsync(pacienteNovo);

                return new OperationResult<string>
                {
                    Status = TypeReturnStatus.Success,
                    Message = "Paciente cadastrado com sucesso."
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<string>(ex, "Erro ao cadastrar paciente.");
            }
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

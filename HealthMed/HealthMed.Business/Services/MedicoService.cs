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
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoRepository _repository;

        public MedicoService(IMedicoRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<Guid?>> CadastroMedico(MedicoCadastroDTO medicoCadastroDTO)
        {
            try
            {
                if (medicoCadastroDTO == null || string.IsNullOrEmpty(medicoCadastroDTO.CRM) || string.IsNullOrEmpty(medicoCadastroDTO.Senha) || string.IsNullOrEmpty(medicoCadastroDTO.NomeCompleto))
                {
                    return PreparaRetornoCadastro("CRM, Nome e Senha são obrigatórios.", TypeReturnStatus.Error);
                }

                var medicoExistente = await _repository.GetByCRMAsync(medicoCadastroDTO.CRM);

                if (medicoExistente != null)
                {
                    return PreparaRetornoCadastro("Já existe um médico cadastrado com este CRM.", TypeReturnStatus.Conflict);
                }

                var medico = new Medico
                {
                    CRM = medicoCadastroDTO.CRM,
                    Nome = medicoCadastroDTO.NomeCompleto,
                    IdEspecialidade = medicoCadastroDTO.IdEspecialidade,
                    Senha = BCrypt.Net.BCrypt.HashPassword(medicoCadastroDTO.Senha),
                    Id = Guid.NewGuid()
                };

                await _repository.AddAsync(medico);

                return PreparaRetornoCadastro("Medico cadastrado com Sucesso", TypeReturnStatus.Success, medico.Id);
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<Guid?>(ex, "Erro ao cadastrar médico.");
            }
        }

        public async Task<OperationResult<MedicoLoginRetornoDTO?>> Login(MedicoLoginDTO medicoLoginDTO)
        {
            try
            {
                if (medicoLoginDTO == null || string.IsNullOrEmpty(medicoLoginDTO.CRM) || string.IsNullOrEmpty(medicoLoginDTO.Senha))
                {
                    return PreparaRetornoLogin("CRM e Senha são obrigatórios.", TypeReturnStatus.Error);
                }

                var medico = await _repository.GetByCRMAsync(medicoLoginDTO.CRM);

                if (medico == null || !BCrypt.Net.BCrypt.Verify(medicoLoginDTO.Senha, medico.Senha))
                {
                    return PreparaRetornoLogin("CRM ou Senha inválidos.", TypeReturnStatus.Error);
                }

                string tokenString = GerarJwtToken(medico);

                var loginReturn = new MedicoLoginRetornoDTO
                {
                    NomeMedico = medico.Nome,
                    MedicoId = medico.Id,
                    Token = tokenString
                };

                return PreparaRetornoLogin("Login realizado com sucesso", TypeReturnStatus.Success, loginReturn);
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<MedicoLoginRetornoDTO?>(ex, "Erro ao realizar login.");
            }
        }

        private static string GerarJwtToken(Medico medico)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("D43493B1-FD3A-49DB-83FF-531A61A5313A");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, medico.Id.ToString()),
                    new Claim(ClaimTypes.Name, medico.Nome),
                    new Claim("CRM", medico.CRM),
                    new Claim(ClaimTypes.Role, "Medico")
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        private static OperationResult<Guid?> PreparaRetornoCadastro(string message, TypeReturnStatus operationStatus, Guid? guid = null)
        {
            return new OperationResult<Guid?>
            {
                Message = message,
                Status = operationStatus,
                ResultObject = guid
            };
        }

        private static OperationResult<MedicoLoginRetornoDTO?> PreparaRetornoLogin(string message, TypeReturnStatus operationStatus, MedicoLoginRetornoDTO? informacaoLogin = null)
        {
            return new OperationResult<MedicoLoginRetornoDTO?>
            {
                Message = message,
                Status = operationStatus,
                ResultObject = informacaoLogin
            };
        }

        public async Task<OperationResult<IEnumerable<Guid>>> BuscarPorEspecialidade(Guid especialidade)
        {
            try
            {
                return new OperationResult<IEnumerable<Guid>>
                {
                    Message = "Busca realizada com sucesso",
                    Status = TypeReturnStatus.Success,
                    ResultObject = await _repository.GetByEspecialidadeAsync(especialidade)
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<IEnumerable<Guid>>(ex, "Erro ao buscar medico");
            }
        }
    }
}


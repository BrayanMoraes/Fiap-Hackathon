using HealthMed.Domain.DTO;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Domain.Interfaces.Services
{
    public interface IPacienteService
    {
        Task<OperationResult<string>> LoginAsync(string cpfOrEmail, string senha);
        Task<OperationResult<string>> CadastrarAsync(PacienteCadastroDTO paciente);
    }
}

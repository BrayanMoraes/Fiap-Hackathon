using HealthMed.Domain.Enum;
using HealthMed.Domain.Shared;
using System;

namespace HealthMed.Domain.Shared
{
    public static class ServiceHelper
    {
        public static OperationResult<T> HandleException<T>(Exception ex, string customMessage = null)
        {
            return new OperationResult<T>
            {
                Status = TypeReturnStatus.Error,
                Message = customMessage ?? $"Erro: {ex.Message}"
            };
        }
    }
}

using HealthMed.Domain.Enum;

namespace HealthMed.Domain.Shared
{
    public class OperationResult<T>
    {
        public T ResultObject { get; set; }
        public string Message { get; set; }
        public TypeReturnStatus Status { get; set; }
    }
}

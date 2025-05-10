using HealthMed.Domain.Enum;
using HealthMed.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthMedBaseController : ControllerBase
    {
        public IActionResult TratarRetorno<T>(OperationResult<T> operationResult)
        {
            switch (operationResult.Status)
            {
                case TypeReturnStatus.Error:
                    return BadRequest(operationResult);
                case TypeReturnStatus.Conflict:
                    return BadRequest(operationResult);
                case TypeReturnStatus.Success:
                    return Ok(operationResult);
                default:
                    return BadRequest(operationResult);
            }
        }
    }
}

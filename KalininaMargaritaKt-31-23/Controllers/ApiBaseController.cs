using KalininaMargaritaKt_31_23.Dto;
using Microsoft.AspNetCore.Mvc;

namespace KalininaMargaritaKt_31_23.Controllers
{
    public abstract class ApiBaseController : ControllerBase
    {
        protected IActionResult ToResult<T>(ServiceResult<T> result)
        {
            return result.Status switch
            {
                ServiceStatus.Ok => Ok(result.Data),
                ServiceStatus.NotFound => NotFound(new { error = result.Message }),
                _ => BadRequest(new { error = result.Message })
            };
        }
    }
}
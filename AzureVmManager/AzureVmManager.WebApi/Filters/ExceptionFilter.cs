using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AzureVmManager.WebApi.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult("An unexpected error has occurred.") { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}

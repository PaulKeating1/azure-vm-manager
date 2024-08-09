using AzureVmManager.DataObjects;
using AzureVmManager.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace MailMe8.Server.Filters
{
    public class UserActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var objectId = context.HttpContext.User.GetObjectId();
            var displayName = context.HttpContext.User.GetDisplayName();
            var givenName = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            var surname = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
            ((BaseController)context.Controller).UserDataObject = new User
            {
                ObjectId = objectId ?? string.Empty,
                DisplayName = displayName ?? string.Empty,
                GivenName = givenName ?? string.Empty,
                Surname = surname ?? string.Empty
            };
            await next();
        }
    }
}

using AzureVmManager.DataObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzureVmManager.WebApi.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class BaseController : Controller
    {
        public User? UserDataObject { get; set; }
    }
}

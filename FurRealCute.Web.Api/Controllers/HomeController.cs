using Microsoft.AspNetCore.Mvc;

namespace FurRealCute.Web.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get() => Ok("Hello, Mario. The princess is in another castle.");
}
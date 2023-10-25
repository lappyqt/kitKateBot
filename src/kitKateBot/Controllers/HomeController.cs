using Microsoft.AspNetCore.Mvc;

namespace kitKateBot.Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    public IActionResult Index() => Ok();
}
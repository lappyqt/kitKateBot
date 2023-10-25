using kitKateBot.Services;
using Microsoft.AspNetCore.Mvc;

namespace kitKateBot.Controllers;

[ApiController]
[Route("[controller]")]
public class TwitchController : ControllerBase
{
    private readonly ITwitchService _service;

    public TwitchController(ITwitchService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(string code)
    {
        await _service.RequestAccessToken(code);
        return Ok("Authorization succeeded 💜");
    }
}
using Microsoft.AspNetCore.Mvc;
using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MusicController : ControllerBase
{
    private readonly ILogger<MusicController> _logger;

    public MusicController(ILogger<MusicController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public Music GetRandomMusic()
    {
        return new();
    }
}

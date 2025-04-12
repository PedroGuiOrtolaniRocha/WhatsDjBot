using Microsoft.AspNetCore.Mvc;
using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<MusicController> _logger;

    public UserController(ILogger<MusicController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public Music GetRandomMusic()
    {
        return new();
    }

    [HttpGet]
    public Music GetRandomUser()
    {
        return new();
    }
}

using Microsoft.AspNetCore.Mvc;
using WhatsDjBotAPI.Models;
using WhatsDjBotAPI.Repositorys;
using WhatsDjBotAPI.Utils;

namespace WhatsDjBotAPI.Controllers;

[ApiController]

[Route("[controller]")]

public class MusicController : ControllerBase
{
    private readonly ILogger<MusicController> _logger;

    private readonly IUserRepository _userRepository;
    private readonly IMusicRepository _musicRepository;
    private readonly IMessageRepository _messageRepository;
    public MusicController(ILogger<MusicController> logger, IUserRepository userRepository, IMusicRepository musicRepository, IMessageRepository messageRepository)
    {
        _logger = logger;
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _musicRepository = musicRepository;
    }

    [HttpGet("GetRamdomMusicFromPlatform")]
    public async Task<MusicResponse> GetRandomMusicFromPlatform([FromQuery] string? platform)
    {
        string[] platforms = new string[] { "Youtube", "Spotfy", "Deezer", "Soundcloud"};
        int musicId;

        if(platform == null || !platforms.Contains<string>(platform)) 
        {
            musicId = await _musicRepository.GetRandomMusicId(); 
        } 
        else
        {
            musicId = await _musicRepository.GetRandomMusicIdByPlatform(platform);
        }

        MusicResponse musicResponse = new MusicResponse(musicId, _musicRepository, _userRepository, _messageRepository);

        return musicResponse;
    }

    [HttpGet("GetRamdomMusic")]
    public async Task<MusicResponse> GetRandomMusic()
    {
        int musicId = await _musicRepository.GetRandomMusicId();
        
        MusicResponse musicResponse = new MusicResponse(musicId, _musicRepository, _userRepository, _messageRepository);

        return musicResponse;
    }


    [HttpPost]
    public async Task<IActionResult> InsertMusic([FromBody] MusicRequest request)
    {
        var user = await _userRepository.GetUserByPhone(request.Phone);
        if (user == null)
        {
            user = new User
            {
                Name = request.UserName,
                Phone = request.Phone
            };
            await _userRepository.InsertUser(user);
        }



        var message = new Message
        {
            UserId = user.Id,
            DateTime = request.DateTime

        };

        await _messageRepository.InsertMessage(message);

        var music = new Music
        {
            Link = request.Link,
            Platform = MessageTools.WhereLinkFrom(request.Link),
            MessageId = message.Id
        };

        await _musicRepository.InsertMusic(music);
        
        Console.WriteLine($"MusicID: {music.Id}");

        return Ok();   
    }
}

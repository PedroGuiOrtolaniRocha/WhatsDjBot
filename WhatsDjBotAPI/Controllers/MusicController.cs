using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
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
    private readonly string[] _platforms;
    public MusicController(ILogger<MusicController> logger, IUserRepository userRepository, IMusicRepository musicRepository, IMessageRepository messageRepository)
    {
        _platforms = new string[] { "Youtube", "Spotify", "Deezer", "Soundcloud" };
        _logger = logger;
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _musicRepository = musicRepository;
    }

    [HttpGet("GetRamdomMusicFromPlatform")]
    public async Task<IActionResult?> GetRandomMusicFromPlatform([FromQuery] string? platform, string groupId)
    {
        int? musicId;
        MusicResponse? musicResponse;


        if (platform == null || !_platforms.Contains<string>(platform)) 
        {
            musicId = await _musicRepository.GetRandomMusicId(groupId);
            if(musicId == null) return NotFound($"Nenhuma música encontrada para a plataforma {platform}.");
            musicResponse = await MusicResponse.CreateAsync(musicId.Value, _musicRepository, _userRepository, _messageRepository);
            return Ok(musicResponse);
        } 
        else
        {
            musicId = await _musicRepository.GetRandomMusicIdByPlatform(platform, groupId);
            if (musicId == null) return NotFound($"Nenhuma música encontrada para a plataforma {platform}.");
            musicResponse = await MusicResponse.CreateAsync(musicId.Value, _musicRepository, _userRepository, _messageRepository);
            return Ok(musicResponse);
        }

    }

    [HttpGet("GetRamdomMusic")]
    public async Task<IActionResult?> GetRandomMusic(string groupId)
    { 
        int? musicId = await _musicRepository.GetRandomMusicId(groupId);
        if (musicId == null) return NotFound($"Nenhuma música encontrada para o grupo.");

        var musicResponse = await MusicResponse.CreateAsync(musicId.Value, _musicRepository, _userRepository, _messageRepository);

        return Ok(musicResponse);
    }


    [HttpPost]
    public async Task<IActionResult> InsertMusic([FromBody] MusicRequest request)
    {
        int userId;

        string? link = MessageTools.GetUrl(request.Link);
        if (link == null)
        {
            return Ok("A mensagem não contem link");
        }

        var user = await _userRepository.GetUserByPhone(request.Phone);
        if (user == null)
        {
            user = new User
            {
                Phone = request.Phone
            };
            user.Name = request.UserName;
            userId = await _userRepository.InsertUser(user);
            user.Id = userId;

        }



        var message = new Message
        {
            UserId = user.Id,
            DateTime = request.DateTime

        };

        await _messageRepository.InsertMessage(message);

        var music = new Music
        {
            Link = link,
            Platform = MessageTools.WhereLinkFrom(request.Link),
            MessageId = message.Id,
            GroupId = request.GroupId
        };

        await _musicRepository.InsertMusic(music, user);
        
        Console.WriteLine($"MusicID: {music.Id}");

        return Ok("Adicionei a musica, muito obridado :D");   
    }
}

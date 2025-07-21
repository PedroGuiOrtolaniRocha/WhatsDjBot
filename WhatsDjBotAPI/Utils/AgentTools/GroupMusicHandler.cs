using System.Collections.Generic;
using WhatsDjBotAPI.Interfaces;
using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI.Utils.AgentTools;


public class GroupMusicHandler : IGroupMusicHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IMusicRepository _musicRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly string[] _platforms;
    public GroupMusicHandler(IUserRepository userRepository, IMusicRepository musicRepository, IMessageRepository messageRepository)
    {
        _platforms = new string[] { "Youtube", "Spotify", "Deezer", "Soundcloud" };
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _musicRepository = musicRepository;
    }

    public async Task<List<Message>?> GetMessagesHistory(string userName, string userPhone, int limit = 10)
    {
        Console.WriteLine($"GetMessagesHistory called with userName: {userName}, userPhone: {userPhone}, limit: {limit}");

        User user = await _userRepository.GetUserByPhone(userPhone) ?? new() {Phone = userPhone, Name = userName };

        if (user.Id == null)
        {
            user.Id = await _userRepository.InsertUser(user);
            return null;
        }

        List<Message> messages = await _messageRepository.GetLastMessagesByUser(user.Id, limit);
        return messages;
    }

    public async Task InsertContextMessageAndResponse(ContextMessage message, string response)
    {
        Console.WriteLine($"Inserindo mensagem");

        User user = await _userRepository.GetUserByPhone(message.UserNumber) ?? new User
        {
            Phone = message.UserNumber,
            Name = message.UserName
        };

        if (user.Id == null)
        {
            user.Id = await _userRepository.InsertUser(user);
        }

        Message messageToInsert = new()
        {
            UserId = user.Id,
            texto_user = message.Message,
            texto_bot = response
        };

        await _messageRepository.InsertMessage(messageToInsert);

        Console.WriteLine($"Mensagem inserida com ID: {messageToInsert.Id}");
    }

    public async Task<string?> GetRandomGroupMusic(string? platform, string groupId)
    {
        Console.WriteLine($"GetRandomGroupMusic called with platform: {platform} and groupId: {groupId}");

        int? musicId;
        MusicResponse? musicResponse;


        if (platform == null || !_platforms.Contains(platform))
        {
            musicId = await _musicRepository.GetRandomMusicId(groupId);
            if (musicId == null) return $"Nenhuma música encontrada.";
            musicResponse = await MusicResponse.CreateAsync(musicId.Value, _musicRepository, _userRepository, _messageRepository);
            return System.Text.Json.JsonSerializer.Serialize(musicResponse);
        }
        else
        {
            musicId = await _musicRepository.GetRandomMusicIdByPlatform(platform, groupId);
            if (musicId == null) return $"Nenhuma música encontrada para a plataforma {platform}.";
            musicResponse = await MusicResponse.CreateAsync(musicId.Value, _musicRepository, _userRepository, _messageRepository);
            return System.Text.Json.JsonSerializer.Serialize(musicResponse);
        }

    }

    
    public async Task VeryfyMessageAndInsertMusic(string messageText, string phone, string userName, string groupId)
    {
        int userId;

        string? link = MessageTools.GetUrl(messageText);
        if (link == null)
        {
            return;
        }

        var user = await _userRepository.GetUserByPhone(phone);
        if (user == null)
        {
            user = new User
            {
                Phone = phone
            };
            user.Name = userName;
            userId = await _userRepository.InsertUser(user);
            user.Id = userId;

        }



        var message = new Message
        {
            UserId = user.Id,
        };

        await _messageRepository.InsertMessage(message);

        var music = new Music
        {
            Link = link,
            Platform = MessageTools.WhereLinkFrom(messageText),
            MessageId = message.Id,
            GroupId = groupId
        };

        await _musicRepository.InsertMusic(music, user);

        Console.WriteLine($"MusicID: {music.Id}");

        return;
    }
}

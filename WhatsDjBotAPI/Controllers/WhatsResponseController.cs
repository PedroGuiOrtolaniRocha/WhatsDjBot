using Microsoft.AspNetCore.Mvc;
using System.Text;
using WhatsDjBotAPI;
using WhatsDjBotAPI.Interfaces;
using WhatsDjBotAPI.Models;
using WhatsDjBotAPI.Utils;
using WhatsDjBotAPI.Utils.AgentTools;


namespace WhatsBot.Controllers;
public class WhatsResponseController : ControllerBase
{
    private readonly IGroupMusicHandler _gmHandler;
    private readonly IChatGenerator _chatGenerator;
    private BotSettings _bot;

    public delegate void MessagingEventsHandler(ContextMessage contextMessage);
    private event MessagingEventsHandler onMessagingReciveing;
    private event MessagingEventsHandler onMessagingResponse;


    public WhatsResponseController(IUserRepository userRepository, IMusicRepository musicRepository, IMessageRepository messageRepository, IChatGenerator chatGenerator)
    {
        _gmHandler = new GroupMusicHandler(userRepository, musicRepository, messageRepository);
        _bot = new();
        _chatGenerator = chatGenerator;

        onMessagingReciveing += LogHandler.LogOnMessageReciveing;
        onMessagingResponse += LogHandler.LogOnMessageResponse;

        Console.WriteLine("controler iniciado");
    }

    [HttpGet]
    [Route("api/whatsresponse/messages-upsert")]
    public IActionResult Get()
    {
        return Ok("WhatsResponseController is running. GET");
    }

    [HttpPatch]
    [Route("api/whatsresponse/messages-upsert")]
    public IActionResult Patch()
    {
        return Ok("WhatsResponseController is running. PATCH");
    }

    [HttpPut]
    [Route("api/whatsresponse/messages-upsert")]
    public IActionResult Put()
    {
        return Ok("WhatsResponseController is running. PUT");
    }


    [HttpPost]
    [Route("api/whatsresponse/messages-upsert")]
    public async Task<IActionResult> RecieveAndProcessWhtasappMessage()
    {
        if (HttpContext.Request.Body == null || !HttpContext.Request.Body.CanRead)
        {
            return BadRequest("Request body is empty or not readable.");
        }

        List<Message> messages = new List<Message>();

        HttpContext.Request.EnableBuffering();

        string body = "";


        if (HttpContext.Request.Body.CanSeek)
        {
            HttpContext.Request.Body.Seek(0, System.IO.SeekOrigin.Begin);
            using StreamReader reader = new(HttpContext.Request.Body, Encoding.UTF8, false, 1024, true);
            body = await reader.ReadToEndAsync();
        }

        Dictionary<string, object>? reqDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(body);

        if (_bot.BotId == null)
        {
            _bot.BotSet(
                reqDict["instance"].ToString(),
                reqDict["sender"].ToString(),
                reqDict["server_url"].ToString(),
                reqDict["apikey"].ToString()
            );
        }

        ContextMessage? contextMessage = new(reqDict["data"], _bot);

        if (contextMessage.FromBot)
        {
            return Ok();
        }


        onMessagingReciveing.Invoke(contextMessage);

        if (contextMessage.IsGroup)
        {
            await _gmHandler.VeryfyMessageAndInsertMusic(contextMessage.Message, contextMessage.UserNumber, contextMessage.UserName, contextMessage.GroupId);

            if (contextMessage.IsResponse || contextMessage.IsMentioned)
            {
                await sendMessage(contextMessage);
            }

        }
        if (!contextMessage.IsGroup)
        {
            await sendMessage(contextMessage);
        }

        return Ok();
    }

    private async Task sendMessage(ContextMessage contextMessage)
    {
        List<Message>? messageHistory = await _gmHandler.GetMessagesHistory(contextMessage.UserName, contextMessage.UserNumber, 10);

        contextMessage.BotResponse = await _chatGenerator.GenerateChatResponseAsync(contextMessage.Message, contextMessage.UserName, contextMessage.GroupId, _bot.BotName, _gmHandler, messageHistory);
        await contextMessage.SendResponse();
        await _gmHandler.InsertContextMessageAndResponse(contextMessage);

        onMessagingResponse.Invoke(contextMessage);
    }

}


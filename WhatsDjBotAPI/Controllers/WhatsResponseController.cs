using Microsoft.AspNetCore.Mvc;
using System.Text;
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

    public WhatsResponseController(IUserRepository userRepository, IMusicRepository musicRepository, IMessageRepository messageRepository, IChatGenerator chatGenerator)
    {
        _gmHandler = new GroupMusicHandler(userRepository, musicRepository, messageRepository);
        _bot = new();
        _chatGenerator = chatGenerator;
    }

    [HttpPost]
    [Route("api/whatsresponse/messages-upsert")]
    public async Task<IActionResult> RecieveAndProcessWhtasappMessage()
    {
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


        logMessage(contextMessage);

        if (contextMessage.IsGroup)
        {
            await _gmHandler.VeryfyMessageAndInsertMusic(contextMessage.Message, contextMessage.UserNumber, contextMessage.UserName, contextMessage.GroupId);

            if (contextMessage.IsResponse || contextMessage.IsMentioned)
            {
                List<Message>? messageHistory = await _gmHandler.GetMessagesHistory(contextMessage.UserName, contextMessage.UserNumber, 10);

                string outputMessage = await _chatGenerator.GenerateChatResponseAsync(contextMessage.Message, contextMessage.UserName, contextMessage.GroupId, _bot.BotName,_gmHandler, messageHistory);
                await contextMessage.SendResponse(outputMessage);
                await _gmHandler.InsertContextMessageAndResponse(contextMessage, outputMessage);

                Console.WriteLine("Mensagem enviada: " + outputMessage + "\n\n");
            }

        }
        if (!contextMessage.IsGroup)
        {
            List<Message>? messageHistory = await _gmHandler.GetMessagesHistory(contextMessage.UserName, contextMessage.UserNumber, 10);

            string outputMessage = await _chatGenerator.GenerateChatResponseAsync(contextMessage.Message, contextMessage.UserName, null, _bot.BotName, _gmHandler, messageHistory);
            await contextMessage.SendResponse(outputMessage);
            await _gmHandler.InsertContextMessageAndResponse(contextMessage, outputMessage);

            Console.WriteLine("Mensagem enviada: " + outputMessage + "\n\n");
        }

        return Ok();
    }

    private void logMessage(ContextMessage contextMessage)
    {
        Console.WriteLine("\u001b[32m  _   _                   __  __                                            \r\n | \\ | |                 |  \\/  |                                           \r\n |  \\| | _____   ____ _  | \\  / | ___ _ __  ___  __ _  __ _  ___ _ __ ___   \r\n | . ` |/ _ \\ \\ / / _` | | |\\/| |/ _ \\ '_ \\/ __|/ _` |/ _` |/ _ \\ '_ ` _ \\  \r\n | |\\  | (_) \\ V / (_| | | |  | |  __/ | | \\__ \\ (_| | (_| |  __/ | | | | | \r\n |_| \\_|\\___/ \\_/ \\__,_| |_|  |_|\\___|_| |_|___/\\__,_|\\__, |\\___|_| |_| |_| \r\n                                                       __/ |                \r\n                                                      |___/                 \u001b[0m");

        Console.WriteLine("Informações do bot:");
        Console.WriteLine($"Bot Name: {_bot.BotName}");
        Console.WriteLine($"Bot Number: {_bot.BotNumber}\n\n");

        Console.WriteLine("Informações do usuário:");
        Console.WriteLine($"User Name: {contextMessage.UserName}");
        Console.WriteLine($"User ID: {contextMessage.UserId}");
        Console.WriteLine($"User Number: {contextMessage.UserNumber}\n\n");

        Console.WriteLine("Informações da mensagem:");
        Console.WriteLine($"Message: {contextMessage.Message}");
        Console.WriteLine($"Is Mentioned: {contextMessage.IsMentioned}");
        Console.WriteLine($"Is Response: {contextMessage.IsResponse}");

        if (contextMessage.IsGroup)
        {
            Console.WriteLine($"Group ID: {contextMessage.GroupId}\n\n");
        }
        else
        {
            Console.WriteLine("This is a private message.\n\n");
        }
    }

}


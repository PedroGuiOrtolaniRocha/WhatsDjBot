using Microsoft.AspNetCore.Mvc;
using System.Text;
using WhatsDjBotAPI.Utils;

namespace WhatsBot.Controllers;
public class WhatsResponseController : ControllerBase
{
    private BotSettings _bot = new();

    [HttpPost]
    [Route("api/whatsresponse/messages-upsert")]
    public async Task<IActionResult> ReadResponse()
    {
        HttpContext.Request.EnableBuffering();

        string body = "";

        Console.WriteLine(HttpContext.Request.Body.CanSeek);

        if (HttpContext.Request.Body.CanSeek)
        {
            HttpContext.Request.Body.Seek(0, System.IO.SeekOrigin.Begin);
            using StreamReader reader = new(HttpContext.Request.Body, Encoding.UTF8, false, 1024, true);
            body = await reader.ReadToEndAsync();
        }

        Dictionary<string, object> reqDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(body);
        foreach (var item in reqDict)
        {
            Console.WriteLine($"Key: {item.Key}\nValue: {item.Value}\n\n");
        }

        if (_bot.BotId == null)
        {
            _bot.BotSet(
                reqDict["instance"].ToString(),
                reqDict["sender"].ToString() 
            );
        }

        Console.WriteLine($"Bot Name: {_bot.BotName}");
        Console.WriteLine($"Bot ID: {_bot.BotId}");
        Console.WriteLine($"Bot Number: {_bot.BotNumber}");

        ContextMessage contextMessage = new(reqDict["data"], _bot);

        Console.WriteLine($"Message: {contextMessage.Message}");
        Console.WriteLine($"User Name: {contextMessage.UserName}");
        Console.WriteLine($"User ID: {contextMessage.UserId}");
        Console.WriteLine($"User Number: {contextMessage.UserNumber}");
        Console.WriteLine($"Is Mentioned: {contextMessage.IsMentioned}");
        Console.WriteLine($"Is Response: {contextMessage.IsResponse}");
        Console.WriteLine($"Is Group: {contextMessage.IsGroup}");
        if (contextMessage.IsGroup)
        {
            Console.WriteLine($"Group ID: {contextMessage.GroupId}");
        }
        else
        {
            Console.WriteLine("This is a private message.");
        }

        var response = new
        {
            Message = "Hello from WhatsBot!",
            Timestamp = DateTime.UtcNow
        };
        return Ok(response);
    }


}


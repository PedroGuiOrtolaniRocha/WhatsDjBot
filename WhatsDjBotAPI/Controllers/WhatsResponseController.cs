using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace WhatsBot.Controllers;
public class WhatsResponseController : ControllerBase
{
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

        var response = new
        {
            Message = "Hello from WhatsBot!",
            Timestamp = DateTime.UtcNow
        };
        return Ok(response);
    }


}


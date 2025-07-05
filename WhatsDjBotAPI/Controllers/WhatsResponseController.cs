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

        var reqList = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string ,Dictionary<string, string>>>(body);

        foreach(var item in reqList)
        {
            Console.WriteLine($"Key: {item.Key}");
            foreach (var kvp in item.Value)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }
        Console.WriteLine("Received JSON: " );

        var response = new
        {
            Message = "Hello from WhatsBot!",
            Timestamp = DateTime.UtcNow
        };
        return Ok(response);
    }


}


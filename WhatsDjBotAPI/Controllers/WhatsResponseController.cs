using Microsoft.AspNetCore.Mvc;

namespace WhatsBot.Controllers;
public class WhatsResponseController : ControllerBase
{
    [HttpPost]
    [Route("api/whatsresponse/messages-upsert")]
    public IActionResult ReadResponse(string json)
    {
        Console.WriteLine("cai aq");
        Console.WriteLine("Received JSON: " + HttpContext.Request.Body.ToString() + json);

        var response = new
        {
            Message = "Hello from WhatsBot!",
            Timestamp = DateTime.UtcNow
        };
        return Ok(response);
    }
}


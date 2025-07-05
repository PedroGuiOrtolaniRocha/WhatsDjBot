using Microsoft.AspNetCore.Mvc;

namespace WhatsBot.Controllers;
public class WhatsResponseController : ControllerBase
{
    [HttpPost]
    [Route("api/whatsresponse/messages-upsert")]
    public IActionResult ReadResponse()
    {
        Console.WriteLine("cai aq");
        Console.WriteLine("Received Header: " + HttpContext.Request.Headers.ToString());

        Console.WriteLine("Received JSON: " + HttpContext.Request.Body.ToString());

        var response = new
        {
            Message = "Hello from WhatsBot!",
            Timestamp = DateTime.UtcNow
        };
        return Ok(response);
    }
}


using Microsoft.AspNetCore.Mvc;

namespace WhatsBot.Controllers;
public class WhatsResponseController : ControllerBase
{
    [HttpPost]
    [Route("api/whatsresponse/messages-upsert")]
    public async Task<IActionResult> ReadResponse()
    {
        var rawMessage = await HttpContext.Request.ReadFromJsonAsync<String>();

        Console.WriteLine("Received JSON: " + rawMessage);

        var response = new
        {
            Message = "Hello from WhatsBot!",
            Timestamp = DateTime.UtcNow
        };
        return Ok(response);
    }


}


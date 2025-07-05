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

        Console.WriteLine("Received JSON: " + HttpContext.Request.Body + GetRequestBody(HttpContext));

        var response = new
        {
            Message = "Hello from WhatsBot!",
            Timestamp = DateTime.UtcNow
        };
        return Ok(response);
    }

    public static string GetRequestBody(HttpContext httpContext)
    {
        var bodyStream = new StreamReader(httpContext.Request.Body);
        bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
        var bodyText = bodyStream.ReadToEnd();
        return bodyText;
    }
}


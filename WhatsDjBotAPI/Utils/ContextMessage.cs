using System.Text;

namespace WhatsDjBotAPI.Utils
{
    public class ContextMessage
    {
        public string Message { get; private set; }
        public string UserName { get; private set; }
        public string UserId { get; private set; }
        public string? GroupId { get; private set; }
        public string UserNumber { get; private set; }
        public bool IsMentioned { get; private set; }
        public bool IsResponse { get; private set; }
        public bool IsGroup { get; private set; }
        public bool FromBot { get; private set; }
        private readonly BotSettings _bot;

        public ContextMessage(object messageDataObj, BotSettings bot)
        {
            _bot = bot;

            Dictionary<string, object> messageData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(messageDataObj.ToString());
            Dictionary<string, object> messageKey = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(messageData["key"].ToString());
            Dictionary<string, object> messageInfo = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(messageData["message"].ToString());


            IsGroup = messageData["key"].ToString().Contains("@g.us");
            Message = messageInfo["conversation"].ToString();
            UserName = messageData["pushName"].ToString();

            if (messageKey["fromMe"].ToString() == "true")
            { 
                UserName = _bot.BotName;
                UserId = _bot.BotId;
                UserNumber = _bot.BotNumber;
                IsMentioned = false;
                IsResponse = false;
                FromBot = true;
            }
            else if(IsGroup)
            {
                IsResponse = messageData["contextInfo"].ToString().Contains(_bot.BotId) && messageData["contextInfo"].ToString().Contains("quotedMessage");
                UserId = messageKey["participant"].ToString();
                GroupId = messageKey.ContainsKey("remoteJid") ? messageKey["remoteJid"].ToString() : string.Empty;
                IsMentioned = Message.Contains("@" + _bot.BotNumber);
            }

            else
            {
                IsMentioned = false;
                IsResponse = false;
                UserId = messageKey["remoteJid"].ToString();
                GroupId = null;
            }

            UserNumber = UserId.Substring(0, 13);
        }

        public async Task SendResponse(string outputMessage)
        {

            var messagePayload = new
            {
                number = GroupId ?? UserNumber,
                text = outputMessage
            };

            HttpClient client = new();

            HttpRequestMessage request = new(HttpMethod.Post, _bot.ServerUrl + "/message/sendText/" + _bot.BotName);
            request.Headers.Add("apikey", _bot.ApiKey);
            request.Content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(messagePayload),
                Encoding.UTF8,
                "application/json"
            );

            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode) // Se o status não for 2xx (Sucesso)
            {
                string errorContent = await response.Content.ReadAsStringAsync(); // <-- LÊ O CONTEÚDO DO ERRO
                Console.WriteLine($"Erro HTTP {response.StatusCode}: {errorContent}");
            }
        }
    }
}

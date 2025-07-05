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
        private readonly BotSettings _bot;

        public ContextMessage(object messageDataObj, BotSettings bot)
        {
            _bot = bot;

            Console.WriteLine("ContextMessage Constructor Called");
            Dictionary<string, object> messageData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(messageDataObj.ToString());
            Dictionary<string, object> messageKey = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(messageData["key"].ToString());
            Dictionary<string, object> messageInfo = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(messageData["message"].ToString());


            Console.WriteLine("Message Data, Info and Key Deserialized");
            IsGroup = messageData["key"].ToString().Contains("@g.us");

            Message = messageInfo["conversation"].ToString();
            UserName = messageData["pushName"].ToString();
            Console.WriteLine($"variaveis setadas");
            IsMentioned = Message.Contains("@" + _bot.BotNumber);

            if (IsGroup)
            {
                IsResponse = messageData["contextInfo"].ToString().Contains(_bot.BotId) && messageData["contextInfo"].ToString().Contains("quotedMessage");
                UserId = messageKey["participant"].ToString();
                GroupId = messageKey.ContainsKey("remoteJid") ? messageKey["remoteJid"].ToString() : string.Empty;
            }

            else
            {
                IsResponse = false;
                UserId = messageKey["remoteJid"].ToString();
                GroupId = null;
            }

            UserNumber = UserId.Substring(0, 13);
        }
    }
}

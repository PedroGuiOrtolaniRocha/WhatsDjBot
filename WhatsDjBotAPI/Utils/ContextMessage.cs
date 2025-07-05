namespace WhatsDjBotAPI.Utils
{
    public class ContextMessage
    {
        public string Message { get; private set; }
        public string UserName { get; private set; }
        public string UserId { get; private set; }
        public string GroupId { get; private set; }
        public string UserNumber { get; private set; }
        public bool IsMentioned { get; private set; }
        public bool IsResponse { get; private set; }
        public bool IsGroup { get; private set; }
        private readonly BotSettings _bot;

        public ContextMessage(object messageDataObj, BotSettings bot)
        {
            _bot = bot;
            Dictionary<string, object> messageData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(messageDataObj.ToString());
            Dictionary<string, object> messageKey = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(messageData["key"].ToString());

            GroupId = messageKey.ContainsKey("remoteJid") ? messageKey["remoteJid"].ToString() : string.Empty;
            IsGroup = messageData["key"].ToString().Contains("@g.us");

            Dictionary<string, string> messageInfo = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(messageData["message"].ToString());


            Message = messageInfo["conversation"];
            UserName = messageData["pushName"].ToString();
            UserId = messageKey["participant"].ToString();
            UserNumber = UserId.Substring(0, 13);

            IsMentioned = Message.Contains("@" + _bot.BotNumber);

            if (IsGroup)
            {
                IsResponse = messageData["contextInfo"].ToString().Contains(_bot.BotId) && messageData["contextInfo"].ToString().Contains("quotedMessage");
            }
            else
            {
                IsResponse = messageData["messageContextInfo"].ToString().Contains(_bot.BotId) && messageData["messageContextInfo"].ToString().Contains("quotedMessage");
            }
        }
    }
}

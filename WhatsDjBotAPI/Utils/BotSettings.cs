namespace WhatsDjBotAPI.Utils
{
    public class BotSettings
    {
        public string? BotName { get; private set; }
        public string? BotId { get; private set; }
        public string? BotNumber { get; private set; }
        public string? ServerUrl { get; private set; }
        public string? ApiKey { get; private set; }


        public BotSettings() 
        {
            BotId = null;
            BotName = null;
            BotNumber = null;
        }
        public void BotSet(string botName, string botId, string serverUrl, string apiKey) 
        { 
            BotName = botName;
            BotId = botId;
            BotNumber = botId.Substring(0, 13);
            ServerUrl = serverUrl;
            ApiKey = apiKey;
        }
    }
}

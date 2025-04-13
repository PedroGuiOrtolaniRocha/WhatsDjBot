namespace WhatsDjBotAPI.Utils
{
    public class MessageTools
    {
        public static string WhereLinkFrom(string url) 
        {
            string platform = "Desconhecido";

            if(url.Contains("youtube.com") || url.Contains("youtu.be")) { platform = "Youtube"; }
            if(url.Contains("spotify.com")) { platform = "Spotify"; }
            if(url.Contains("soundcloud.com")) { platform = "Soundcloud"; }
            if(url.Contains("deezer.com")) { platform = "Deezer"; }
            
            return platform;
        }

        private static bool ContainsUrl(string message)
        {
            
            if (message.Contains("https://"))
            {
                return true;
            }
            
            return false;
        }

        public static string? GetUrl(string message)
        {

            if (!ContainsUrl(message)) { return null; }

            int startindex = message.IndexOf("https://");
            int endindex = message.IndexOf(" ", startindex);
            if (endindex == -1) { endindex = message.Length; }
            
            return message.Substring(startindex, endindex);
        }
    }
}

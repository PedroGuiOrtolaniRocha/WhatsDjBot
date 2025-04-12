namespace WhatsDjBotAPI.Utils
{
    public class MessageTools
    {
        public static string WhereLinkFrom(string url) 
        {
            string platform = "Desconhecido";

            if(url.Contains("youtube.com")) { platform = "Youtube"; }
            if(url.Contains("spotify.com")) { platform = "Spotfy"; }
            if(url.Contains("soundcloud.com")) { platform = "SoundClound"; }
            if(url.Contains("deezer.com")) { platform = "Deezer"; }
            
            return platform;
        }
    }
}

namespace WhatsDjBotAPI.Utils
{
    public class MessageTools
    {
        public static string[] PossiblePlatforms = ["youtu.be", "spotify.com", "youtube.com", "soundcloud.com", "deezer.com"
        ];
        public static string WhereLinkFrom(string url)
        {
            var platform = "Desconhecido";

            if (url.Contains("youtube.com") || url.Contains("youtu.be") || url.Contains("music.youtube")) { platform = "Youtube"; }
            if (url.Contains("spotify.com")) { platform = "Spotify"; }
            if (url.Contains("soundcloud.com")) { platform = "Soundcloud"; }
            if (url.Contains("deezer.com")) { platform = "Deezer"; }

            return platform;
        }

        private static bool ContainsUrl(string message)
        {

            if (message.Contains("https://") || message.Contains("http://") || message.Contains("www."))
            {
                return true;
            }

            foreach (var platform in PossiblePlatforms)
            {
                if (message.Contains(platform))
                {
                    return true;
                }
            }

            return false;
        }

        public static string? GetUrl(string message)
        {

            if (!ContainsUrl(message)) { return null; }

            var startindex = -1;

            if (message.Contains("https://"))
            {
                startindex = message.IndexOf("https://");
                message = message.Substring(startindex);
            }

            if (startindex == -1 && message.Contains("http://"))
            {

                message = message.Replace("http://", "https://");
                startindex = message.IndexOf("https://");
                message = message.Substring(startindex);
            }

            if (startindex == -1 && message.Contains("www."))
            {
                message = message.Replace("www.", "https://");
                startindex = message.IndexOf("https://");
                message = message.Substring(startindex);

            }

            if (startindex == -1)
            {
                foreach (var platform in PossiblePlatforms)
                {
                    if (startindex == -1 && message.Contains(platform))
                    {
                        startindex = message.IndexOf(platform);
                        message = "https://" + message;
                        startindex = 0;
                        break;
                    }
                }
                if (startindex == -1) { return null; }
            }


            int endindex = message.IndexOf(" ");
            if (endindex == -1) { endindex = message.Length; }

            string url = message.Substring(0, endindex - startindex);

            return url;
        }
    }
}

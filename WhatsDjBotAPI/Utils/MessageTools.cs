namespace WhatsDjBotAPI.Utils
{
    public class MessageTools
    {
        public static string[] PossiblePlatforms = new string[] { "youtu.be", "spotify.com", "youtube.com", "soundcloud.com", "deezer.com" };
        public static string WhereLinkFrom(string url)
        {
            string platform = "Desconhecido";

            if (url.Contains("youtube.com") || url.Contains("youtu.be")) { platform = "Youtube"; }
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

            foreach (string platform in PossiblePlatforms)
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

            int startindex = message.IndexOf("https://");

            if (startindex == -1)
            {
                startindex = message.IndexOf("http://");
            }

            if (startindex == -1)
            {
                startindex = message.IndexOf("www.");

                if (startindex == -1)
                {
                    foreach(string platform in PossiblePlatforms)
                    {
                        if (startindex == -1)
                        {
                            if (message.Contains(platform))
                            {
                                startindex = message.IndexOf(platform);
                                break;
                            }
                        }
                    }
                    if (startindex == -1) { return null; }
                }
            }

            message = message.Substring(startindex);

            int endindex = message.IndexOf(" ");
            if (endindex == -1) { endindex = message.Length; }

            Console.WriteLine($"\n" +
                $"\n\nStart: {startindex} End: {endindex}\n\n");

            return message.Substring(startindex, endindex);
        }
    }
}

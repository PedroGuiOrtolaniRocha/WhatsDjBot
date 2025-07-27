using WhatsDjBotAPI.Utils;

namespace WhatsDjBotAPI
{
    public static class LogHandler
    {
        public static void LogOnStart()
        {
            Console.Clear();
            Console.WriteLine("\x1b[32m           _____ _____     ____        _ _            \r\n     /\\   |  __ \\_   _|   / __ \\      | (_)           \r\n    /  \\  | |__) || |    | |  | |_ __ | |_ _ __   ___ \r\n   / /\\ \\ |  ___/ | |    | |  | | '_ \\| | | '_ \\ / _ \\\r\n  / ____ \\| |    _| |_   | |__| | | | | | | | | |  __/\r\n /_/    \\_\\_|   |_____|   \\____/|_| |_|_|_|_| |_|\\___|\r\n                                                      \r\n                                                      \x1b[0m");
        }

        public static void LogOnMessageReciveing(ContextMessage contextMessage)
        {
            Console.Clear();
            Console.WriteLine(
                "\u001b[32m  _   _                   __  __                                            \r\n | \\ | |                 |  \\/  |                                           \r\n |  \\| | _____   ____ _  | \\  / | ___ _ __  ___  __ _  __ _  ___ _ __ ___   \r\n | . ` |/ _ \\ \\ / / _` | | |\\/| |/ _ \\ '_ \\/ __|/ _` |/ _` |/ _ \\ '_ ` _ \\  \r\n | |\\  | (_) \\ V / (_| | | |  | |  __/ | | \\__ \\ (_| | (_| |  __/ | | | | | \r\n |_| \\_|\\___/ \\_/ \\__,_| |_|  |_|\\___|_| |_|___/\\__,_|\\__, |\\___|_| |_| |_| \r\n                                                       __/ |                \r\n                                                      |___/                 \u001b[0m");

            Console.WriteLine("Informações do usuário:");
            Console.WriteLine($"User Name: {contextMessage.UserName}");
            Console.WriteLine($"User ID: {contextMessage.UserId}");
            Console.WriteLine($"User Number: {contextMessage.UserNumber}\n\n");

            Console.WriteLine("Informações da mensagem:");
            Console.WriteLine($"Message: {contextMessage.Message}");
            Console.WriteLine($"Is Mentioned: {contextMessage.IsMentioned}");
            Console.WriteLine($"Is Response: {contextMessage.IsResponse}");

            if (contextMessage.IsGroup)
            {
                Console.WriteLine($"Group ID: {contextMessage.GroupId}\n\n");
            }
            else
            {
                Console.WriteLine("This is a private message.\n\n");
            }
        }

        public static void LogOnMessageResponse(ContextMessage contextMessage)
        {
            Console.WriteLine($"\n\n\x1b[34m Mensagem enviada para {contextMessage.UserName} ({contextMessage.UserNumber}):\n{contextMessage.BotResponse}\x1b[0m");
        }
    }
}

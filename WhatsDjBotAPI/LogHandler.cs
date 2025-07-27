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
                "\u001b[32m \n\n - - - - - - - - - Mensagem Recebida - - - - - - - - - \n\n \u001b[0m");


            Console.WriteLine("Informações da mensagem:");
            Console.WriteLine($"Mensagem enviada por {contextMessage.UserName} ({contextMessage.UserNumber})\n" +
                              $"\u001b[32m{contextMessage.Message}\u001b[0m");
            if (contextMessage.IsMentioned)
            {
                Console.WriteLine("A mensagem menciona o bot");
            }

            if (contextMessage.IsResponse)
            {
                Console.WriteLine("A mensagem responde o bot");
            }

            if (contextMessage.IsGroup)
            {
                Console.WriteLine($"Group ID: {contextMessage.GroupId}\n\n");
            }

        }

        public static void LogOnMessageResponse(ContextMessage contextMessage)
        {
            Console.WriteLine(
                "\x1b[32m \n\n - - - - - - - - - Mensagem Enviada - - - - - - - - - \n\n \x1b[0m");
            Console.WriteLine($" Mensagem enviada para {contextMessage.UserName} ({contextMessage.UserNumber})" +
                $"\x1b[34m\n{contextMessage.BotResponse}\x1b[0m");
        }
    }
}

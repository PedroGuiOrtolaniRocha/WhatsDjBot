using WhatsDjBotAPI.Utils;

namespace WhatsDjBotAPI
{
    public static class LogHandler
    {
        private static string setColor(string colorName, string text)
        {
            int colorCode;
            switch (colorName.ToLower())
            {
                case "red":
                    colorCode = 31;
                    break;
                case "green":
                    colorCode = 32;
                    break;
                case "yellow":
                    colorCode = 33;
                    break;
                case "blue":
                    colorCode = 34;
                    break;
                case "magenta":
                    colorCode = 35;
                    break;
                case "cyan":
                    colorCode = 36;
                    break;
                case "white":
                    colorCode = 37;
                    break;
                default:
                    colorCode = 0; // Default terminal color
                    break;
            }

            return $"\u001b[{colorCode}m{text}\u001b[0m";
        }
        public static void LogOnStart()
        {
            Console.Clear();
            Console.WriteLine(setColor("green",
                "           _____ _____     ____        _ _            \r\n" +
                "     /\\   |  __ \\_   _|   / __ \\      | (_)           \r\n" +
                "    /  \\  | |__) || |    | |  | |_ __ | |_ _ __   ___ \r\n" +
                "   / /\\ \\ |  ___/ | |    | |  | | '_ \\| | | '_ \\ / _ \\\r\n" +
                "  / ____ \\| |    _| |_   | |__| | | | | | | | | |  __/\r\n" +
                " /_/    \\_\\_|   |_____|   \\____/|_| |_|_|_|_| |_|\\___|\r\n" +
                "                                                      \r\n" +
                "                                                      "));
        }

        public static void LogOnMessageReciveing(ContextMessage contextMessage)
        {
            Console.Clear();
            Console.WriteLine(setColor("cyan",
                "\n\n - - - - - - - - - Mensagem Recebida - - - - - - - - - \n\n"));

            Console.WriteLine("Mensagem enviada por" +
                              setColor("cyan",
                                  $" {contextMessage.UserName} ") + "(" +
                              setColor("cyan",
                                  $"{contextMessage.UserNumber}") + ")\n" +
                              setColor("cyan",
                              $"{contextMessage.Message}")
                              );

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
            Console.WriteLine(setColor("blue",
                "\n\n - - - - - - - - - Mensagem Enviada - - - - - - - - - \n\n"));

            Console.WriteLine(" Mensagem enviada para" +
                              setColor("blue",
                                  $" {contextMessage.UserName} ") + "(" +
                              setColor("blue",
                                  $"{contextMessage.UserNumber}") + ")\n"
            );

        }

        public static void LogOnAiChatGenerate(object? sender, EventArgs e)
        {
            Console.WriteLine(setColor("yellow",
                " - - - - - - - - - Preparando resposta - - - - - - - - - \n\n"));
        }
        public static void LogOnAiToolUse(string toolName, string[] args)
        {
            Console.WriteLine($"\nFerramente usada:" +
                              setColor("yellow",
                                $"{toolName}"));

            for (int i = 0; i < args.Length; i += 2)
            {
                Console.WriteLine("Parâmetro: " +
                                  setColor("yellow", $"{args[i]} ") +
                                  "Valor: " +
                                  setColor("yellow", $"{args[i + 1]}"));
            }
        }
    }
}

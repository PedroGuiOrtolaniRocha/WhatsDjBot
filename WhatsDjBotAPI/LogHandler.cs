using WhatsDjBotAPI.Utils;

namespace WhatsDjBotAPI
{
    public static class LogHandler
    {
        private static string SetColor(string colorName, string text)
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
            Console.WriteLine(SetColor("green",
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
            string groupLog = "";

            if (contextMessage.GroupId != null)
            {
                groupLog = " no grupo " + SetColor("cyan", contextMessage.GroupId);
            }

            Console.Clear();
            Console.WriteLine(SetColor("cyan",
                "\n\n - - - - - - - - - Mensagem Recebida - - - - - - - - - \n"));

            Console.WriteLine("Mensagem enviada por" +
                              SetColor("cyan",
                                  $" {contextMessage.UserName} ") + "(" +
                              SetColor("cyan",
                                  $"{contextMessage.UserNumber}") +
                              $"){groupLog}\n\n" +
                              SetColor("cyan",
                              $"{contextMessage.Message}\n")
                              );

            if (contextMessage.IsMentioned || contextMessage.IsResponse)
            {
                if (contextMessage.IsMentioned)
                {
                    Console.WriteLine("A mensagem menciona o bot");
                }
                if (contextMessage.IsResponse)
                {
                    Console.WriteLine("A mensagem responde o bot");
                }
            }
            else
            {
                Console.WriteLine( SetColor("red", "A mensagem não gera resposta"));   
            }


        }

        public static void LogOnMessageResponse(ContextMessage contextMessage)
        {
            string groupLog = "";

            if (contextMessage.GroupId != null)
            {
                groupLog = " no grupo " + SetColor("blue", contextMessage.GroupId);
            }

            Console.WriteLine(SetColor("blue",
                "\n\n - - - - - - - - - Mensagem Enviada - - - - - - - - - \n"));

            Console.WriteLine(" Mensagem enviada para" +
                              SetColor("blue",
                                  $" {contextMessage.UserName} ") + "(" +
                              SetColor("blue",
                                  $"{contextMessage.UserNumber}") + 
                              $"){groupLog}\n\n" + 
                              SetColor("blue",
                                  $"{contextMessage.BotResponse}")
            );

        }

        public static void LogOnAiChatGenerate(object? sender, EventArgs e)
        {
            Console.WriteLine(SetColor("yellow",
                "\n\n - - - - - - - - - Preparando resposta - - - - - - - - - \n"));
        }
        public static void LogOnAiToolUse(string toolName, string[] args)
        {
            Console.WriteLine($"\nFerramente usada:" +
                              SetColor("yellow",
                                $"{toolName}"));

            for (int i = 0; i < args.Length; i += 2)
            {
                Console.WriteLine("Parâmetro: " +
                                  SetColor("yellow", $"{args[i]} ") +
                                  "Valor: " +
                                  SetColor("yellow", $"{args[i + 1]}"));
            }
        }
    }
}

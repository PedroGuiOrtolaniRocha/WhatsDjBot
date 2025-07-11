using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WhatsDjBotAPI.Utils
{
    public class ChatGenerator
    {
        public static async Task<string> GenerateChatResponseAsync(string message, string name)
        {

            OpenAIClientOptions options  = new OpenAIClientOptions();
            options.Endpoint = new Uri("https://api.groq.com/openai/v1");
            IChatClient chatClient = new OpenAIClient(new ApiKeyCredential(Environment.GetEnvironmentVariable("GROQ_API_KEY")),options).GetChatClient(Environment.GetEnvironmentVariable("LLM_MODEL")).AsIChatClient();



            List<OpenAI.Chat.ChatTool> chatTools = new List<OpenAI.Chat.ChatTool>
            {
                OpenAI.Chat.ChatTool.CreateFunctionTool(
                    functionName: "GetMusicsByArtistLastFm", // Nome único para a ferramenta
                    functionDescription: "Fetches the top tracks for a given artist from Last.fm. Use this tool when the user asks for songs, top tracks, or music by a specific artist. The 'qtnd' parameter specifies how many songs to retrieve (defaulting to 5 if not specified by the user).", // Descrição para o modelo
                    functionParameters: BinaryData.FromString("""
                            {
                                "type": "object",
                                "properties": {
                                    "artistName": {
                                        "type": "string",
                                        "description": "The name of the artist to search for."
                                    },
                                    "qtnd": {
                                        "type": "integer",
                                        "description": "The number of top tracks to retrieve (e.g., 5, 10). Defaults to 5 if not provided by the user.",
                                        "default": 5
                                    }
                                },
                                "required": ["artistName"]
                            }
                            """),
                        functionSchemaIsStrict: false
                    )
               
            };

            List<ChatMessage> chatHistory =
            [
                new ChatMessage(ChatRole.System, $"""
                    Seu nome é '01 da porra toda', 
                    seu intuito é ofender ao maximo qualquer um que interaja com você,
                    lembre-se de sempre manter o desrespeito e utilizar {name} para se referir ao usuário,
                    incluindo palavras de baixo calão, aqui estão alguns exemplos 
                    Alem desses, crie variaçõess usando essa base
                    """
                )
            ];

            chatHistory.Add(new ChatMessage(ChatRole.User, message));

            string response = "";
            await foreach (ChatResponseUpdate item in
                chatClient.GetStreamingResponseAsync(chatHistory))
            {
                response += item.Text;
            }
            chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));

            return response.Trim();
        }
    }
}

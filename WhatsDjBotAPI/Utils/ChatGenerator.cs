using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;

namespace WhatsDjBotAPI.Utils
{
    public class ChatGenerator
    {
        public static async Task<string> GenerateChatResponseAsync(string message, string name)
        {

            OpenAIClientOptions options  = new OpenAIClientOptions();
            options.Endpoint = new Uri("https://api.groq.com/openai/v1");
            IChatClient chatClient = new OpenAIClient(new ApiKeyCredential(Environment.GetEnvironmentVariable("GROQ_API_KEY")),options).GetChatClient(Environment.GetEnvironmentVariable("LLM_MODEL")).AsIChatClient();
            
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

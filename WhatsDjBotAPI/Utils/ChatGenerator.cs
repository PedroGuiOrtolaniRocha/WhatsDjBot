using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;
using System.Text.Json.Serialization;

namespace WhatsDjBotAPI.Utils
{
    public class ChatGenerator
    {
        public static async Task<string> GetMusicsByArtistLastFm(string artistName, int qtnd)
        {

            Console.WriteLine($"\n\n\nBuscando músicas do artista: {artistName} com quantidade: {qtnd}\n\n\n");

            string apiKey = Environment.GetEnvironmentVariable("LASTFM_APIKEY") ?? throw new Exception("LASTFM_APIKEY environment variable is not set.");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"http://ws.audioscrobbler.com/2.0/?method=artist.gettoptracks&artist={artistName}&api_key={apiKey}&limit={qtnd}&format=json");
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }
        public static async Task<string> GenerateChatResponseAsync(string message, string name)
        {

            OpenAIClientOptions options  = new OpenAIClientOptions();
            options.Endpoint = new Uri("https://api.groq.com/openai/v1");

            ChatOptions chatOptions = new ChatOptions 
            { 
                Tools = [AIFunctionFactory.Create(async (string artistName, int qtnd) => 
                {
                    return await GetMusicsByArtistLastFm(artistName, qtnd);
                },
                "GetMusicsByArtistLastFm",
                """
                 A ferramenta é usada para buscar as músicas mais populares ou "top tracks" de um artista específico no Last.fm.
                 Use esta ferramenta sempre que o usuário perguntar sobre:
                 - Músicas de um artista (ex: "músicas do Iron Maiden")
                 - Top músicas / faixas de um artista (ex: "quais as top músicas do Queen")
                 - Discoteca / repertório de um artista (ex: "me mostre o que o Led Zeppelin já fez")
                 - Para encontrar faixas ou canções por nome de artista.
                 O parâmetro 'qtnd' (quantidade) pode ser usado para especificar o número de músicas a serem retornadas,
                 com um padrão de 5 se não for especificado pelo usuário.
                 """)],
            };

            IChatClient chatClient = new ChatClientBuilder(new OpenAIClient(new ApiKeyCredential(Environment.GetEnvironmentVariable("GROQ_API_KEY")),options).GetChatClient(Environment.GetEnvironmentVariable("LLM_MODEL")).AsIChatClient()).UseFunctionInvocation().Build();


            List<ChatMessage> chatHistory =
            [
                new ChatMessage(ChatRole.System, Environment.GetEnvironmentVariable("SYSTEM_PROMPT"))
            ];

            chatHistory.Add(new ChatMessage(ChatRole.User, message));

            string response = "";
            await foreach (ChatResponseUpdate item in
                chatClient.GetStreamingResponseAsync(chatHistory, chatOptions))
            {
                response += item.Text;
            }
            chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));

            return response.Trim();
        }
    }
}

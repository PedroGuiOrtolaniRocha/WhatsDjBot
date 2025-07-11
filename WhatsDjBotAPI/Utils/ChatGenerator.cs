using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WhatsDjBotAPI.Utils
{
    public class ChatGenerator
    {
        private class GetMusicsByArtistLastFmArgs
        {
            [JsonPropertyName("artistName")]
            public string? RecipientName { get; set; }

            [JsonPropertyName("qtnd")]
            public int Message { get; set; }
        }
        public static async Task<string> GetMusicsByArtistLastFm(string artistName, int qtnd)
        {

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
            IChatClient chatClient = new OpenAIClient(new ApiKeyCredential(Environment.GetEnvironmentVariable("GROQ_API_KEY")),options).GetChatClient(Environment.GetEnvironmentVariable("LLM_MODEL")).AsIChatClient();



            List<OpenAI.Chat.ChatTool> chatTools = new List<OpenAI.Chat.ChatTool>
            {
                OpenAI.Chat.ChatTool.CreateFunctionTool(
                    functionName: "GetMusicsByArtistLastFm", // Nome único para a ferramenta
                    functionDescription: """
                         A ferramenta é usada para buscar as músicas mais populares ou "top tracks" de um artista específico no Last.fm.
                         Use esta ferramenta sempre que o usuário perguntar sobre:
                         - Músicas de um artista (ex: "músicas do Iron Maiden")
                         - Top músicas / faixas de um artista (ex: "quais as top músicas do Queen")
                         - Discoteca / repertório de um artista (ex: "me mostre o que o Led Zeppelin já fez")
                         - Para encontrar faixas ou canções por nome de artista.
                         O parâmetro 'qtnd' (quantidade) pode ser usado para especificar o número de músicas a serem retornadas,
                         com um padrão de 5 se não for especificado pelo usuário.
                         """,
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
                    Alem desses, crie variaçõess usando essa base.

                    Caso o usuário solicite músicas, use a ferramenta GetMusicsByArtistLastFm para buscar as músicas do artista mencionado, mande o json que retornou da ferramenta.
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

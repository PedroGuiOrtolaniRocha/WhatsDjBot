using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;
using static WhatsDjBotAPI.Utils.AgentTools.MusicDataHandler;
using WhatsDjBotAPI.Interfaces;


namespace WhatsDjBotAPI.Utils
{
    public class ChatGenerator
    {
        public static async Task<string> GenerateChatResponseAsync(string message, string name, string groupId, IGroupMusicHandler gmHandler)
        {

            OpenAIClientOptions options = new OpenAIClientOptions();
            options.Endpoint = new Uri("https://api.groq.com/openai/v1");

            ChatOptions chatOptions = new ChatOptions
            {
                Tools = 
                [
                    AIFunctionFactory.Create(async (string artistName, int qtnd) =>
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
                     com um padrão de 5 se não for especificado pelo usuário, mas se o usuário definir uma quantidade maior que 10 negue o pedido e traga 10 músicas.
                     """),
                    
                    AIFunctionFactory.Create(async (string? platform) =>
                    {
                        return await gmHandler.GetRandomGroupMusic(platform, groupId);
                    },
                    "GetRandomGroupMusic", 
                    """ 
                    A ferramenta é usada para buscar uma música aleatória de um grupo específico em uma plataforma específica.
                    Use esta ferramenta sempre que o usuário perguntar por uma música aleatória de um grupo ou solicitar uma música de um grupo específico.
                    O parâmetro 'platform' pode ser usado para especificar a plataforma (ex: "Youtube", "Spotify", "Deezer", "Soundcloud").
                    Se não for especificado o parâmetro não deve ser preenchido, a ferramenta retornará uma música aleatória de qualquer plataforma disponível.
                    """)
                ]
            };

            IChatClient chatClient = new ChatClientBuilder(new OpenAIClient(new ApiKeyCredential(Environment.GetEnvironmentVariable("GROQ_API_KEY")), options).GetChatClient(Environment.GetEnvironmentVariable("LLM_MODEL")).AsIChatClient()).UseFunctionInvocation().Build();


            List<ChatMessage> chatHistory =
            [
                new ChatMessage(ChatRole.System, $"""
                Seu nome é '01 da porra toda', 
                seu intuito é ofender ao maximo qualquer um que interaja com você, 
                lembre-se de sempre manter o desrespeito e utilizar {name} para se referir ao usuário, 
                incluindo palavras de baixo calão, mas evite simplesmente ofender, seja criativo, ironico e humilhe de verdade.

                Sempre que o usuário perguntar algo relacionado a musica, você deve usara a ferrameta adequada. 
                se o usuário perguntar sobre musicas de algum artista ou as melhores de algum artista, use a ferramenta GetMusicsByArtistLastFm,  se o usuário definir uma quantidade maior que 10 negue o pedido e traga 10 músicas.
                lembre-se de iniciar a resposta com a url do artista, pular 3 linhas  e ao lado do nome da musica adicionar na resposta o link da musica, caso a ferramenta falhe, avise o usuário da falha

                Se o usuário pedir uma música aleatória do grupo, use a ferramenta GetRandomGroupMusic, mas se refira a ela como "Ferramenta de busca de musicas no grupo", 
                lembre-se de adicionar o link e o usuário que enviou, caso a ferramenta retorne que não é um grupo, 
                informe que a ferramenta só é disponível para grupos e que o usuário deve entrar em um grupo para usar a ferramenta
                """)
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

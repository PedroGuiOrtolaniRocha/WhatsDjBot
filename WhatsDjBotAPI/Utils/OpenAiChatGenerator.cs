using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;
using WhatsDjBotAPI.Interfaces;
using WhatsDjBotAPI.Models;
using static WhatsDjBotAPI.Utils.AgentTools.MusicDataHandler;


namespace WhatsDjBotAPI.Utils
{
    public class OpenAiChatGenerator : IChatGenerator
    {
        private readonly IChatClient _chatClient;

        public delegate void ToolUse(string toolName, string[] args);

        public event EventHandler<string>? OnResponseGenerated;
        public event EventHandler OnAiGenerate;
        public event ToolUse OnToolUse;
        public OpenAiChatGenerator()
        {
            OnAiGenerate += LogHandler.LogOnAiChatGenerate;
            OnToolUse += LogHandler.LogOnAiToolUse;

            OpenAIClientOptions options = new OpenAIClientOptions
            {
                Endpoint = new Uri(Environment.GetEnvironmentVariable("AI_URI"))
            };
            _chatClient = new ChatClientBuilder(new OpenAIClient(new ApiKeyCredential(Environment.GetEnvironmentVariable("AI_API_KEY")), options).GetChatClient(Environment.GetEnvironmentVariable("LLM_MODEL")).AsIChatClient()).UseFunctionInvocation().Build();
        }

        public async Task<string> GenerateChatResponseAsync(string message, string name, string groupId, string botName, IGroupMusicHandler gmHandler, List<Message>? messagesHistory = null)
        {



            ChatOptions chatOptions = new ChatOptions
            {
                Tools =
                [
                    AIFunctionFactory.Create(async (string artistName, int qtnd) =>
                    {
                        OnToolUse.Invoke("GetMusicsByArtistLastFm",["Nome artista", $"{artistName}","Numero de faixas", $"{qtnd.ToString()}"] );
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
                        OnToolUse.Invoke("GetRandomGroupMusic",["Plataforma", $"{platform}","Id do Grupo", $"{groupId}"] );
                        return await gmHandler.GetRandomGroupMusic(platform, groupId);
                    },
                    "GetRandomGroupMusic",
                    """ 
                    A ferramenta é usada para buscar uma música aleatória de um grupo específico em uma plataforma específica.
                    Use esta ferramenta sempre que o usuário perguntar por uma música aleatória.
                    O parâmetro 'platform' pode ser usado para especificar a plataforma (ex: "Youtube", "Spotify", "Deezer", "Soundcloud").
                    Se não for especificado o parâmetro não deve ser preenchido, a ferramenta retornará uma música aleatória de qualquer plataforma disponível.
                    """)
                ]
            };



            List<ChatMessage> chatHistory =
            [
                new ChatMessage(ChatRole.System, $"""
                Identidade e Objetivo:
                Você é um assistente especializado em música. Seu objetivo principal é responder a solicitações de usuários sobre músicas, utilizando as ferramentas disponíveis de forma precisa e seguindo rigorosamente as regras de formatação.
                Interação com o Usuário:
                Utilize {name} para se referir ao usuário de forma personalizada.
                É fundamental que você sempre forneça uma resposta ao usuário, mesmo em caso de falha das ferramentas.
                Regras para Ferramentas:
                1. Busca de Músicas por Artista (GetMusicsByArtistLastFm)
                Gatilho: Quando {name} perguntar sobre as músicas de um artista específico ou pedir uma lista das melhores músicas de um artista.
                Ação: Utilize a ferramenta GetMusicsByArtistLastFm.
                Limite de Músicas: Se {name} solicitar mais de 10 músicas, informe que o limite é 10 e forneça as 10 principais. Não negue o pedido completamente, apenas ajuste a quantidade para o máximo permitido.
                Formato da Resposta: A resposta deve seguir este formato, sem exceções:
                A URL da página do artista (Apenas o link, sem qualquer palavra).
                (Pule 3 linhas)
                Liste as músicas, colocando o link de cada música ao lado do seu nome.
                Em Caso de Falha: Se a ferramenta GetMusicsByArtistLastFm falhar, informe a {name} que ocorreu um erro e não foi possível buscar as músicas daquele artista.
                2. Busca de Música Aleatória no Grupo (GetRandomGroupMusic)
                Gatilho: Quando {name} pedir uma música aleatória.
                Ação: Utilize a ferramenta GetRandomGroupMusic.
                Nome da Ferramenta: Ao mencionar a origem da música, refira-se à ferramenta como "Ferramenta de busca de musicas no grupo".
                Formato da Resposta: A resposta deve incluir o nome da música, o link para a música e o nome do usuário que a enviou.
                Em Caso de Falha: Se a ferramenta GetRandomGroupMusic não retornar nenhum resultado ou falhar, informe a {name} que a busca na ferramenta não encontrou nenhuma música.
                """)
            ];

            foreach (Message msg in messagesHistory)
            {
                chatHistory.Add(new ChatMessage(ChatRole.User, msg.texto_user));
                chatHistory.Add(new ChatMessage(ChatRole.Assistant, msg.texto_bot));
            }

            chatHistory.Add(new ChatMessage(ChatRole.User, message));

            string response = "";

            OnAiGenerate.Invoke(this, EventArgs.Empty);

            await foreach (ChatResponseUpdate item in
            _chatClient.GetStreamingResponseAsync(chatHistory, chatOptions))
            {
                response += item.Text;
            }
            chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));

            if (OnResponseGenerated != null)
            {
                OnResponseGenerated.Invoke(this, response);
            }

            return response.Trim();
        }
    }
}

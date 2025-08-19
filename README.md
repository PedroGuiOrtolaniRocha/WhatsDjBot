# WhatsDjAgent

WhatsDjAgent é um Agent A.I para WhatsApp que armazena e recomenda músicas enviadas em grupos, utilizando IA e integração com APIs externas.

## Principais Tecnologias

- **Evolution API** versão 2.2.3 (integração WhatsApp)
- **Entity Framework Core** para acesso ao banco de dados
- **PostgreSQL** como SGDB
- **.NET 8 / ASP.NET Core Web API**
- **OpenAI** (via Microsoft.Extensions.AI)
- **Last.fm API** para busca de músicas populares
- **Sistema hospedado e ativo em nossa VPS**

## Funcionalidades

- Armazenamento automático de links de músicas enviadas em grupos (YouTube, Spotify, Deezer, Soundcloud)
- Recomendações de músicas aleatórias do grupo
- Busca de faixas populares de artistas (Last.fm)
- Histórico de mensagens e interações
- Respostas personalizadas usando IA
- Suporte a múltiplas plataformas: YouTube, Spotify, Deezer, Soundcloud
- **Responde mensagens privadas diretamente**
- **Responde em grupos apenas quando é citado ou quando uma mensagem é uma resposta ao bot**

## Estrutura do Projeto

```
WhatsDjBotAPI/
├── Controllers/
├── Interfaces/
├── Models/
├── Repositorys/
├── Utils/
├── appsettings.json
├── dockerfile
├── Program.cs
└── ...
sql/
README.md
```

## Como Executar

1. Configure o banco de dados PostgreSQL usando o schema em [`sql`](sql).
2. Defina as variáveis de ambiente necessárias:
   - `AI_URI`
   - `AI_API_KEY`
   - `LLM_MODEL`
   - `LASTFM_APIKEY`
3. Ajuste a string de conexão em `appsettings.json`.
4. Execute o projeto:
   ```sh
   dotnet run --project WhatsDjBotAPI/WhatsDjBotAPI.csproj
   ```
5. Para rodar via Docker:
   ```sh
   docker build -t whats-dj-bot-api WhatsDjBotAPI/
   docker run -p 8080:8080 --env-file .env whats-dj-bot-api
   ```

## Endpoints Principais

- `POST /api/whatsresponse/messages-upsert`  
  Recebe e processa mensagens do WhatsApp.

## Licença

MIT

---

O sistema está hospedado e ativo em nossa VPS.

## Criadores

- [Pedro Ortolani](https://github.com/PedroGuiOrtolaniRocha)
- [Henrique Botella](https://github.com/databotella)

Para dúvidas ou sugestões, entre em contato conosco.

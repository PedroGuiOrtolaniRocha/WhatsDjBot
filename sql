-- =================================================================
-- SCRIPT DE BANCO DE DADOS: idadatamart
-- Versão: 2.0
-- Data: 19/08/2025
-- Descrição: Schema completo para o sistema de automação, incluindo
--            interações de usuário, log de mensagens, músicas e
--            histórico de chat do n8n.
-- =================================================================

-- Passo 1: Criação do Banco de Dados
-- Conecte-se ao seu servidor Postgres com um superusuário para executar.
-- Se o banco de dados já existir, você pode pular esta parte.
CREATE DATABASE idadatamart
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    CONNECTION LIMIT = -1;

-- =================================================================
-- AVISO: Os comandos a seguir devem ser executados DENTRO do banco 'idadatamart'.
-- No terminal psql, use o comando: \c idadatamart
-- =================================================================

-- =========================================
-- TABELA: User
-- Descrição: Armazena informações dos usuários que interagem com o sistema.
-- =========================================
CREATE TABLE IF NOT EXISTS public."User"
(
    "Id" SERIAL PRIMARY KEY,                    -- Identificador único e automático para cada usuário.
    "Name" character varying(255) NOT NULL,     -- Nome do usuário (ex: nome do contato no WhatsApp).
    "Phone" character varying(20) NOT NULL      -- Número de telefone do usuário (formato internacional, ex: 5511999999999).
);

COMMENT ON TABLE public."User" IS 'Usuários que interagem com as automações.';
COMMENT ON COLUMN public."User"."Id" IS 'Chave primária da tabela de usuários.';
COMMENT ON COLUMN public."User"."Name" IS 'Nome do usuário, geralmente obtido do WhatsApp.';
COMMENT ON COLUMN public."User"."Phone" IS 'Número de telefone em formato internacional.';


-- =========================================
-- TABELA: Message
-- Descrição: Registra cada mensagem trocada, ligando o usuário ao conteúdo.
-- =========================================
CREATE TABLE IF NOT EXISTS public."Message"
(
    "Id" SERIAL PRIMARY KEY,                                    -- Identificador único e automático para cada mensagem.
    "UserId" integer NOT NULL,                                  -- Referência ao usuário que enviou a mensagem (FK para User.Id).
    "DateTime" timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP, -- Data e hora exatas em que a mensagem foi registrada.
    texto_bot text,                                             -- Conteúdo da resposta enviada pelo bot.
    texto_user text                                             -- Conteúdo da mensagem original enviada pelo usuário.
);

COMMENT ON TABLE public."Message" IS 'Log de todas as mensagens trocadas entre usuários e o bot.';
COMMENT ON COLUMN public."Message"."UserId" IS 'ID do usuário que iniciou a interação.';
COMMENT ON COLUMN public."Message"."DateTime" IS 'Timestamp de quando a mensagem foi processada.';
COMMENT ON COLUMN public."Message".texto_bot IS 'O texto que o bot enviou como resposta.';
COMMENT ON COLUMN public."Message".texto_user IS 'O texto que o usuário enviou.';


-- =========================================
-- TABELA: Music
-- Descrição: Armazena os links de músicas gerados a partir de uma mensagem.
-- =========================================
CREATE TABLE IF NOT EXISTS public."Music"
(
    "Id" SERIAL PRIMARY KEY,                    -- Identificador único e automático para cada música.
    "Link" character varying(255) NOT NULL,     -- URL da música encontrada (YouTube, Spotify, etc.).
    "Platform" character varying(50) NOT NULL,  -- Plataforma de origem da música (ex: youtube, spotify).
    "GroupId" character varying(255),           -- ID do grupo do WhatsApp onde a música foi pedida (NULL se for em chat privado).
    "MessageId" integer NOT NULL                -- Referência à mensagem que originou a busca desta música (FK para Message.Id).
);

COMMENT ON TABLE public."Music" IS 'Registros de músicas enviadas pelo bot.';
COMMENT ON COLUMN public."Music"."Link" IS 'URL completa da música.';
COMMENT ON COLUMN public."Music"."Platform" IS 'Plataforma de origem (youtube, spotify, etc).';
COMMENT ON COLUMN public."Music"."GroupId" IS 'ID do grupo do WhatsApp (nulo se for conversa privada).';
COMMENT ON COLUMN public."Music"."MessageId" IS 'ID da mensagem que solicitou a música.';


-- =========================================
-- TABELA: n8n_chat_histories
-- Descrição: Armazena o histórico de conversas do n8n para manter o contexto.
-- =========================================
CREATE TABLE IF NOT EXISTS public.n8n_chat_histories
(
    id SERIAL PRIMARY KEY,                      -- Identificador único e automático para o registro.
    session_id character varying(255) NOT NULL, -- Identificador da sessão de conversa (ex: número de telefone).
    message jsonb NOT NULL                      -- O histórico da conversa armazenado em formato JSON.
);

COMMENT ON TABLE public.n8n_chat_histories IS 'Histórico de conversas para Agentes de IA do n8n.';
COMMENT ON COLUMN public.n8n_chat_histories.session_id IS 'ID que agrupa as mensagens de uma mesma conversa (ex: número do usuário).';
COMMENT ON COLUMN public.n8n_chat_histories.message IS 'Array de objetos JSON contendo o histórico da conversa.';


-- =========================================
-- ÍNDICES PARA PERFORMANCE
-- Descrição: Aceleram as consultas mais comuns no banco de dados.
-- =========================================
CREATE INDEX IF NOT EXISTS "IX_Message_UserId" ON public."Message" ("UserId");
CREATE INDEX IF NOT EXISTS "IX_Music_MessageId" ON public."Music" ("MessageId");
CREATE INDEX IF NOT EXISTS "IX_Music_GroupId" ON public."Music" ("GroupId");
CREATE INDEX IF NOT EXISTS "IX_Music_MessageId_GroupId" ON public."Music" ("MessageId", "GroupId");
CREATE INDEX IF NOT EXISTS "IX_Music_Platform" ON public."Music" ("Platform");

COMMENT ON INDEX "IX_Message_UserId" IS 'Acelera a busca de todas as mensagens de um usuário específico.';
COMMENT ON INDEX "IX_Music_MessageId" IS 'Acelera a busca de músicas relacionadas a uma mensagem específica.';


-- =========================================
-- CHAVES ESTRANGEIRAS (RELACIONAMENTOS)
-- Descrição: Garantem a integridade e consistência dos dados entre as tabelas.
-- =========================================
ALTER TABLE IF EXISTS public."Message"
    ADD CONSTRAINT "FK_Message_User" FOREIGN KEY ("UserId")
    REFERENCES public."User" ("Id")
    ON DELETE CASCADE;

ALTER TABLE IF EXISTS public."Music"
    ADD CONSTRAINT "FK_Music_Message" FOREIGN KEY ("MessageId")
    REFERENCES public."Message" ("Id")
    ON DELETE CASCADE;

COMMENT ON CONSTRAINT "FK_Message_User" ON public."Message" IS 'Garante que toda mensagem pertença a um usuário válido. Se o usuário for deletado, suas mensagens também serão.';
COMMENT ON CONSTRAINT "FK_Music_Message" ON public."Music" IS 'Garante que toda música esteja ligada a uma mensagem válida. Se a mensagem for deletada, a música também será.';

-- =========================================
-- EXEMPLOS DE USO
-- =========================================

-- Inserir um novo usuário:
-- INSERT INTO "User" ("Name", "Phone") VALUES ('Henrique', '5511987654321');

-- Inserir uma nova mensagem de um usuário (assumindo que o UserId = 1):
-- INSERT INTO "Message" ("UserId", texto_user) VALUES (1, 'me manda uma musica');

-- Inserir uma música relacionada a uma mensagem (assumindo que o MessageId = 1):
-- INSERT INTO "Music" ("Link", "Platform", "MessageId") VALUES ('https://www.youtube.com/watch?v=dQw4w9WgXcQ', 'youtube', 1);

-- =========================================
-- FIM DO SCRIPT
-- =========================================

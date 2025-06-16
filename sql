-- =========================================
-- MUSIC BOT DATABASE SCHEMA
-- Versão: 1.1 - Com suporte a grupos
-- Data: 16/06/2025
-- Descrição: Schema para sistema de Music Bot no WhatsApp
-- =========================================

-- =========================================
-- TABELA: User
-- Descrição: Armazena informações dos usuários
-- =========================================
CREATE TABLE "User" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,       -- Nome do usuário no WhatsApp
    "Phone" VARCHAR(20) NOT NULL        -- Número de telefone (formato: 5511999999999)
);

-- =========================================
-- TABELA: Message  
-- Descrição: Armazena mensagens enviadas pelos usuários
-- =========================================
CREATE TABLE "Message" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL,          -- Referência ao usuário que enviou
    "GroupId" VARCHAR(255),             -- ID do grupo WhatsApp (NULL = chat privado)
    "DateTime" TIMESTAMP NOT NULL,      -- Data/hora da mensagem
    
    -- Chave estrangeira
    CONSTRAINT "FK_Message_User" FOREIGN KEY ("UserId") 
        REFERENCES "User" ("Id") ON DELETE CASCADE
);

-- =========================================
-- TABELA: Music
-- Descrição: Armazena links de músicas enviadas pelo bot
-- =========================================
CREATE TABLE "Music" (
    "Id" SERIAL PRIMARY KEY,
    "Link" VARCHAR(255) NOT NULL,       -- URL da música (YouTube, Spotify, etc)
    "Platform" VARCHAR(50) NOT NULL,    -- Plataforma (youtube, spotify, apple_music)
    "GroupId" VARCHAR(255),             -- ID do grupo (NULL = chat privado)
    "MessageId" INTEGER NOT NULL,       -- Referência à mensagem que gerou a música
    
    -- Chave estrangeira
    CONSTRAINT "FK_Music_Message" FOREIGN KEY ("MessageId") 
        REFERENCES "Message" ("Id") ON DELETE CASCADE
);

-- =========================================
-- ÍNDICES PARA PERFORMANCE
-- Descrição: Otimizam consultas frequentes
-- =========================================

-- Busca mensagens por usuário
CREATE INDEX "IX_Message_UserId" ON "Message" ("UserId");

-- Busca músicas por mensagem
CREATE INDEX "IX_Music_MessageId" ON "Music" ("MessageId");

-- =========================================
-- COMENTÁRIOS ADICIONAIS
-- =========================================

-- Tabela User
COMMENT ON TABLE "User" IS 'Usuários do sistema Music Bot';
COMMENT ON COLUMN "User"."Name" IS 'Nome do usuário no WhatsApp';
COMMENT ON COLUMN "User"."Phone" IS 'Número de telefone no formato internacional';

-- Tabela Message
COMMENT ON TABLE "Message" IS 'Mensagens enviadas pelos usuários';
COMMENT ON COLUMN "Message"."UserId" IS 'ID do usuário que enviou a mensagem';
COMMENT ON COLUMN "Message"."GroupId" IS 'ID do grupo WhatsApp (NULL para chats privados)';
COMMENT ON COLUMN "Message"."DateTime" IS 'Timestamp da mensagem';

-- Tabela Music
COMMENT ON TABLE "Music" IS 'Músicas encontradas e enviadas pelo bot';
COMMENT ON COLUMN "Music"."Link" IS 'URL da música encontrada';
COMMENT ON COLUMN "Music"."Platform" IS 'Plataforma da música (youtube, spotify, etc)';
COMMENT ON COLUMN "Music"."GroupId" IS 'ID do grupo onde foi enviada (NULL para privado)';
COMMENT ON COLUMN "Music"."MessageId" IS 'ID da mensagem que originou esta música';

-- =========================================
-- EXEMPLOS DE USO
-- =========================================

-- Inserir usuário:
-- INSERT INTO "User" ("Name", "Phone") VALUES ('João Silva', '5511999999999');

-- Inserir mensagem privada:
-- INSERT INTO "Message" ("UserId", "GroupId", "DateTime") VALUES (1, NULL, NOW());

-- Inserir mensagem de grupo:
-- INSERT INTO "Message" ("UserId", "GroupId", "DateTime") VALUES (1, '120363xyz@g.us', NOW());

-- Inserir música:
-- INSERT INTO "Music" ("Link", "Platform", "GroupId", "MessageId") 
-- VALUES ('https://youtube.com/watch?v=abc123', 'youtube', NULL, 1);

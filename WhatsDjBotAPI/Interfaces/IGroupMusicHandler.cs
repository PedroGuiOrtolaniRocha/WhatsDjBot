using WhatsDjBotAPI.Models;
using WhatsDjBotAPI.Utils;

namespace WhatsDjBotAPI.Interfaces
{
    public interface IGroupMusicHandler
    {
        public Task<string?> GetRandomGroupMusic(string? platform, string groupId);
        public Task VeryfyMessageAndInsertMusic(string messageText, string phone, string userName, string groupId);
        public Task<List<Message>?> GetMessagesHistory(string userName, string userPhone, int limit = 10);
        public Task InsertContextMessageAndResponse(ContextMessage message);
    }
}

using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI.Interfaces
{
    public interface IChatGenerator
    {
        public Task<string> GenerateChatResponseAsync(string message, string name, string groupId, IGroupMusicHandler gmHandler, List<Message>? messagesHistory = null);

    }
}

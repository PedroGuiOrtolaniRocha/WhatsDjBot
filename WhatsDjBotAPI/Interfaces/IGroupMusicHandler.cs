namespace WhatsDjBotAPI.Interfaces
{
    public interface IGroupMusicHandler
    {
        public Task<string?> GetRandomGroupMusic(string? platform, string groupId);
        public Task VeryfyMessageAndInsertMusic(string messageText, string phone, string userName, string groupId);
    }
}

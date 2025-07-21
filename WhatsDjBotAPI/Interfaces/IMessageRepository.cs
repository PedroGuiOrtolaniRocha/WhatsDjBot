using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI.Interfaces
{
    public interface IMessageRepository
    {
        public Task<int> InsertMessage(Message message);
        public Task<Message> GetMessageById(int id);
        public Task<List<Message>> GetLastMessagesByUser(int userId, int limit = 10);
    }
}

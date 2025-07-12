using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI.Interfaces
{
    public interface IMessageRepository
    {
        public Task<int> InsertMessage(Message message);
        public Task<Message> GetMessageById(int id);

    }
}

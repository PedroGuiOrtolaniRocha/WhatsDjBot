using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI.Repositorys
{
    public interface IMessageRepository
    {
        public Task<int> InsertMessage(Message message);
    }
}

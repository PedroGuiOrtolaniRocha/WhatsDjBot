using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI.Repositorys
{
    public interface IUserRepository
    {
        public Task<User>? GetUserByPhone(string phone);
        public Task InsertUser(User user);
    }
}

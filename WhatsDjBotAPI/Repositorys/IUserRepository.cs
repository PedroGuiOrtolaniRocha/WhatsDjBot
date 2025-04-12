using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI.Repositorys
{
    public interface IUserRepository
    {
        public Task<User>? GetUserByPhone(string phone);
        public Task<User>? GetUserById(int id);
        public Task InsertUser(User user);
    }
}

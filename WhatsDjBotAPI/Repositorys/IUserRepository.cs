using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI.Repositorys
{
    public interface IUserRepository
    {
        public Task<User> GetUserById(long id);
        public Task InsertUser(User user);
    }
}

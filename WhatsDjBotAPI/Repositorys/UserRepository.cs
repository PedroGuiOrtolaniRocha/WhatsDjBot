using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI.Repositorys
{
    public class UserRepository : IUserRepository
    {
        DbEntity _context;
        public UserRepository(DbEntity context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task InsertUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}

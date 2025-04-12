using Microsoft.EntityFrameworkCore;
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

        public async Task<User>? GetUserByPhone(string phone)
        {
            return await _context.Users.FirstOrDefaultAsync<User>(x => x.Phone == phone );
        }

        public async Task InsertUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}

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

        public async Task<User>? GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync<User>(x => x.Id == id);
        }

        public async Task<User>? GetUserByPhone(string phone)
        {
            return await _context.Users.FirstOrDefaultAsync<User>(x => x.Phone == phone );
        }

        public async Task<int> InsertUser(User user)
        {
            var userExist = await GetUserByPhone(user.Phone);
            if (userExist != null)
            {
                user.Id = userExist.Id;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
    }
}

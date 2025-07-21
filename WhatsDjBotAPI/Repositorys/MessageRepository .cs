using Microsoft.EntityFrameworkCore;
using WhatsDjBotAPI.Interfaces;
using WhatsDjBotAPI.Models;
namespace WhatsDjBotAPI.Repositorys
{
    public class MessageRepository : IMessageRepository
    {
        DbEntity _context;
        public MessageRepository(DbEntity context)
        {
            _context = context;
        }

        public async Task<Message> GetMessageById(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<int> InsertMessage(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message.Id;
        }

        public async Task<List<Message>> GetLastMessagesByUser(int userId, int limit = 10)
        {
            return await _context.Messages
                .Where(m => m.UserId == userId && m.texto_bot != null & m.texto_user != null)
                .OrderByDescending(m => m.DateTime)
                .Take(limit)
                .ToListAsync();
        }
    }
}

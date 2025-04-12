
using Microsoft.EntityFrameworkCore;
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

    }
}

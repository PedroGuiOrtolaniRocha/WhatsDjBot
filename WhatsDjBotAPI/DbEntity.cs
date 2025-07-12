using Microsoft.EntityFrameworkCore;
using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI
{
    public class DbEntity : DbContext
    {
        public DbSet<Music> Musics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbEntity(DbContextOptions<DbEntity> options) : base(options)
        {

        }
    }
}

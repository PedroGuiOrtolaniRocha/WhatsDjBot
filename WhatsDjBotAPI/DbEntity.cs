using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI
{
    public class DbEntity : DbContext
    {
        public DbSet<Music> Musics { get; set; }
        public DbSet<User> Users { get; set; }

        public DbEntity(DbContextOptions<DbEntity> options) : base(options)
        {

        }
    }
}

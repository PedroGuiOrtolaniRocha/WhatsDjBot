
using Microsoft.EntityFrameworkCore;
using WhatsDjBotAPI.Models;
namespace WhatsDjBotAPI.Repositorys
{
    public class MusicRepository : IMusicRepository
    {
        DbEntity _context;
        public MusicRepository(DbEntity context) 
        {
            _context = context;
        }

        public async Task<Music> GetRandomMusic()
        {
            int lenght = await _context.Musics.CountAsync<Music>();

            Music? music =  await _context.Musics.FindAsync(new Random().Next(1, lenght));
            while(music == null) { music = await _context.Musics.FindAsync(new Random().Next(1, lenght)); }
            Console.WriteLine($"ID: {music.Id}");

            return music;
        }

        public async Task InsertMusic(Music music)
        {
            await _context.Musics.AddAsync(music);
            await _context.SaveChangesAsync();
        }

    }
}

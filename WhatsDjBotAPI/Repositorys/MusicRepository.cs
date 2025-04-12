
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

        public async Task<int> GetRandomMusicIdByPlatform(string platform)
        {
            int lenght = await _context.Musics.MaxAsync(x => x.Id);
            int randomId = new Random().Next(1, lenght);
            Console.WriteLine("Plataforma" + platform);
            Music? music = await _context.Musics.FirstOrDefaultAsync<Music>(x => x.Platform == platform && x.Id == randomId);

            return music.Id;
        }
        public async Task<int> GetRandomMusicId()
        {
            int lenght = await _context.Musics.MaxAsync(x => x.Id);
            Console.WriteLine(lenght);
            Music? music =  await _context.Musics.FindAsync(new Random().Next(1, lenght));
            while(music == null) { music = await _context.Musics.FindAsync(new Random().Next(1, lenght + 1)); }
            Console.WriteLine($"ID: {music.Id}");

            return music.Id;
        }

        public async Task<Music> GetMusicById(int id)
        {
            return await _context.Musics.FindAsync(id);
        }

        public async Task InsertMusic(Music music)
        {
            await _context.Musics.AddAsync(music);
            await _context.SaveChangesAsync();
        }

    }
}

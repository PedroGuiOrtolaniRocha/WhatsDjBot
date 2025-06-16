
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.RegularExpressions;
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

        public async Task<int?> GetRandomMusicIdByPlatform(string platform, string groupId)
        {
            Console.WriteLine("Plataforma" + platform);


            List<Music> musics = await _context.Musics.Where(x => x.Platform == platform && x.GroupId == groupId).ToListAsync<Music>();
            int randomIndex = new Random().Next(0, musics.Count());

            Music? music = musics[randomIndex];
            if (music == null)
            {
                return null;
            }
            return music.Id;
        }
        public async Task<int?> GetRandomMusicId(string groupId)
        {
           
            List<Music> groupMusics = _context.Musics.Where(x => x.GroupId == groupId).ToList();
            int randomIndex = new Random().Next(0, groupMusics.Count());
            Music? music = groupMusics[randomIndex];
            if (music == null)
            {
                return null;
            }
            return music.Id;

        }

        public async Task<Music> GetMusicById(int id)
        {
            return await _context.Musics.FindAsync(id);
        }

        public async Task InsertMusic(Music music, User sender)
        {
            bool musicaExistente = false;
            List<Music>? musics = _context.Musics.Where<Music>(x => x.Link == music.Link).ToList();
            if(musics == null)
            {
                await _context.Musics.AddAsync(music);
                await _context.SaveChangesAsync();
            }

            foreach (var item in musics)
            {
                if (item.Link == music.Link && item.GroupId == music.GroupId)
                {
                    musicaExistente = true;
                    return;
                }
            }

            await _context.Musics.AddAsync(music);
            await _context.SaveChangesAsync();
        }

    }
}

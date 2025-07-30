
using Microsoft.EntityFrameworkCore;
using WhatsDjBotAPI.Interfaces;
using WhatsDjBotAPI.Models;
namespace WhatsDjBotAPI.Repositorys
{
    public class MusicRepository : IMusicRepository
    {
        readonly DbEntity _context;
        public MusicRepository(DbEntity context)
        {
            _context = context;
        }

        public async Task<int?> GetRandomMusicIdByPlatform(string platform, string groupId)
        {
            List<Music> musics = await _context.Musics.Where(x => x.Platform == platform && x.GroupId == groupId).ToListAsync<Music>();
            int randomIndex = new Random().Next(0, musics.Count());

            Music? music = musics[randomIndex];
            if (music == null)
            {
                return null;
            }
            return music.Id;
        }
        public async Task<int?> GetRandomMusicId(string? groupId)
        {
            int randomIndex;

            if (groupId == null)
            {
                randomIndex = new Random().Next(0, _context.Musics.Count());
                Music? musicRandom = await _context.Musics.FindAsync(randomIndex);
            }

            List<Music> groupMusics = await _context.Musics.Where(x => x.GroupId == groupId).ToListAsync<Music>();
            randomIndex = new Random().Next(0, groupMusics.Count());
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
            List<Music>? musics = _context.Musics.Where<Music>(x => x.Link == music.Link).ToList();
            if (musics == null)
            {
                await _context.Musics.AddAsync(music);
                await _context.SaveChangesAsync();
            }

            foreach (var item in musics)
            {
                if (item.Link == music.Link && item.GroupId == music.GroupId)
                {
                    return;
                }
            }

            await _context.Musics.AddAsync(music);
            await _context.SaveChangesAsync();
        }

    }
}

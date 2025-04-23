
using Microsoft.EntityFrameworkCore;
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

        public async Task<int> GetRandomMusicIdByPlatform(string platform, string groupId)
        {
            Console.WriteLine("Plataforma" + platform);


            List<Music> musics = await _context.Musics.Where(x => x.Platform == platform && x.groupId == groupId).ToListAsync<Music>();
            int randomIndex = new Random().Next(0, musics.Count());

            Music? music = musics[randomIndex];

            return music.Id;
        }
        public async Task<int> GetRandomMusicId(string groupId)
        {
           
            List<Music> groupMusics = _context.Musics.Where(x => x.groupId == groupId).ToList();
            int randomIndex = new Random().Next(0, groupMusics.Count());
            Music? music = groupMusics[randomIndex];
            Console.WriteLine($"ID: {music.Id}");

            return music.Id;
        }

        public async Task<Music> GetMusicById(int id)
        {
            return await _context.Musics.FindAsync(id);
        }

        public async Task InsertMusic(Music music, User sender)
        {
            List<Music> musics = _context.Musics.Where<Music>(x => x.Link == music.Link).ToList();
            List<Message> messages = new List<Message>();

            if (musics.Count() != 0)
            {
                List<int> usersWhosSendThisMusic = new List<int>();

                foreach (Music m in musics)
                {
                    messages.Add(_context.Messages.Where<Message>(x => x.Id == music.MessageId).First());
                }
                foreach (Message m in messages)
                {
                    usersWhosSendThisMusic.Add(m.UserId);
                }
                if (usersWhosSendThisMusic.Contains(sender.Id))
                {
                    return;
                }
            } 
           
            await _context.Musics.AddAsync(music);
            await _context.SaveChangesAsync();
        }

    }
}

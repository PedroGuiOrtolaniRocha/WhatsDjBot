using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI.Repositorys
{
    public interface IMusicRepository
    {
        public Task<int?> GetRandomMusicIdByPlatform(string platform, string groupId);
        public Task<int?> GetRandomMusicId(string groupId);
        public Task<Music> GetMusicById(int id);
        public Task InsertMusic(Music music, User sender);
    }
}

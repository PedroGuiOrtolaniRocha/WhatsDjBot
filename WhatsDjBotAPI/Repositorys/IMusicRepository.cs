using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI.Repositorys
{
    public interface IMusicRepository
    {
        public Task<int> GetRandomMusicId();
        public Task<Music> GetMusicById(int id);
        public Task InsertMusic(Music music);
    }
}

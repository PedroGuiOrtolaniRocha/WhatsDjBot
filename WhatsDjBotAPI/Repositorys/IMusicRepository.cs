using WhatsDjBotAPI.Models;

namespace WhatsDjBotAPI.Repositorys
{
    public interface IMusicRepository
    {
        public Task<Music> GetRandomMusic();
        public Task InsertMusic(Music music);
    }
}

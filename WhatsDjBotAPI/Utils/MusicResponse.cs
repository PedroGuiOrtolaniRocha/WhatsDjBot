using WhatsDjBotAPI.Repositorys;
using WhatsDjBotAPI.Models;


namespace WhatsDjBotAPI.Utils
{
    public class MusicResponse
    {
        private readonly IMusicRepository _musicRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        public string link { get; set; }
        public string platform { get; set; }
        public string userName { get; set; }

        public MusicResponse(int id, IMusicRepository musicRepository, IUserRepository userRepository, IMessageRepository messageRepository)
        {
            _musicRepository = musicRepository;
            _userRepository = userRepository;
            _messageRepository = messageRepository;

            Music music = _musicRepository.GetMusicById(id).Result;
            Message message = _messageRepository.GetMessageById(music.Id).Result;
            Console.WriteLine(message.UserId);
            User user = _userRepository.GetUserById(message.UserId).Result;

            link = music.Link;
            platform = music.Platform;
            userName = user.Name;

        }
    }
}

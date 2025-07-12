using WhatsDjBotAPI.Repositorys;


namespace WhatsDjBotAPI.Utils
{
    public class MusicResponse
    {
        public string Link { get; set; }
        public string Platform { get; set; }
        public string UserName { get; set; }

        public static async Task<MusicResponse?> CreateAsync(
            int id,
            IMusicRepository musicRepository,
            IUserRepository userRepository,
            IMessageRepository messageRepository)
        {
            var music = await musicRepository.GetMusicById(id);
            if (music == null) return null;

            var message = await messageRepository.GetMessageById(music.MessageId);
            if (message == null) return null;

            var user = await userRepository.GetUserById(message.UserId);
            if (user == null) return null;

            return new MusicResponse
            {
                Link = music.Link,
                Platform = music.Platform,
                UserName = user.Name
            };
        }
    }
}

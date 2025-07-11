using System.Text.Json.Serialization;

namespace WhatsDjBotAPI.Utils
{
    public class MusicDataHandler
    {
        public static async Task<string> GetMusicsByArtistLastFm(string artistName, int qtnd)
        {
            if(qtnd > 10) qtnd = 10;

            Console.WriteLine($"\n\n\nBuscando músicas do artista: {artistName} com quantidade: {qtnd}\n\n\n");

            string apiKey = Environment.GetEnvironmentVariable("LASTFM_APIKEY") ?? throw new Exception("LASTFM_APIKEY environment variable is not set.");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"http://ws.audioscrobbler.com/2.0/?method=artist.gettoptracks&artist={artistName}&api_key={apiKey}&limit={qtnd}&format=json");
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }
    }
}

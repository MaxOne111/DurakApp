using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Durak.Network
{
    public sealed class DurakLobbyService
    {
        public DurakLobbyService(HttpClient client)
        {
            _client = client;
        }

        private readonly string LobbyUrl = DurakHelper.URLGame;
        //"https://duraktest.sino0on.ru/api/lobby";
        ///LobbyUrl = "https://durak.sino0on.ru/api/lobby";
        
        private readonly HttpClient _client;

        public async Task<int> CreateLobby(DurakConfiguration configuration)
        {
            var data = JsonConvert.SerializeObject(configuration);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            
            var isValidUri = Uri.TryCreate(LobbyUrl, UriKind.RelativeOrAbsolute, out var uri);
            if (!isValidUri)
            {
                throw new ArgumentException();
            }
            
            var request = await _client.PostAsync(uri, content);
            var responseJson = await request.Content.ReadAsStringAsync();
            
            configuration = JsonConvert.DeserializeObject<DurakConfiguration>(responseJson);
            return configuration.Id;
        }
        
        public async Task<int> CreateLobby(DurakTournamentConfiguration configuration, string url)
        {
            var data = JsonConvert.SerializeObject(configuration);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            
            var isValidUri = Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri);
            if (!isValidUri)
            {
                throw new ArgumentException();
            }
            
            var request = await _client.PostAsync(uri, content);
            var responseJson = await request.Content.ReadAsStringAsync();
            
            configuration = JsonConvert.DeserializeObject<DurakTournamentConfiguration>(responseJson);
            return configuration.Id;
        }
    }
}
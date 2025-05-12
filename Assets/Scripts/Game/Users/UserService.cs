using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Game.Users.Requests;
using Game.Users.Responses;
using Infrastructure;
using Infrastructure.Enums;
using Newtonsoft.Json;

namespace Game.Users
{
    public sealed class UserService
    {
        public UserService(HttpClient client, EVerboseMode verboseMode = EVerboseMode.Message)
        {
            _client = client;
            _verboseMode = verboseMode;
        }

        private readonly HttpClient _client;
        private readonly EVerboseMode _verboseMode;
        
        private const string BaseUrl = "http://188.225.84.38:8001";
        
        private const string LoginEndpoint = "/api/user/login";
        private const string RegisterEndpoint = "/api/user/registration";

        private async Task<TResponse> SendPostRequest<TRequest, TResponse>(TRequest request, string url)
        {
            var data = JsonConvert.SerializeObject(request);
            
            VerboseProvider.Log($"UserService: trying to send data \"{data}\" to url \"{url}\"", _verboseMode);
            
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            
            var isValidUri = Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri);
            if (!isValidUri)
            {
                throw new ArgumentException();
            }
            
            var responseMessage = await _client.PostAsync(uri, content);
            var responseJson = await responseMessage.Content.ReadAsStringAsync();

            VerboseProvider.Log($"UserService: received message \"{responseJson}\"", _verboseMode);
            
            var result = JsonConvert.DeserializeObject<TResponse>(responseJson);
            return result;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var result = await SendPostRequest<LoginRequest, LoginResponse>
                (request, BaseUrl + LoginEndpoint);
            
            // TODO: RESEARCH
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            
            return result;
        }

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            var result = await SendPostRequest<RegisterRequest, RegisterResponse>
                (request, BaseUrl + RegisterEndpoint);
            
            return result;
        }
    }
}
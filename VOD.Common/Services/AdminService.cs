using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VOD.Common.HttpClients;

namespace VOD.Common.Services
{
    public class AdminService : IAdminService
    {
        readonly MembershipHttpClient _http;

        public AdminService(MembershipHttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<TDto>> GetAsync<TDto>(string uri)
        {
            try
            {
                using HttpResponseMessage response = await _http.Client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var result = JsonSerializer.Deserialize<List<TDto>>(await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result ?? new List<TDto>();
            }
            catch
            {
                throw;
            }
        }

        public async Task<TDto?> SingleAsync<TDto>(string uri)
        {
            try
            {
                using HttpResponseMessage response = await _http.Client.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                var result = JsonSerializer.Deserialize<TDto>(await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result ?? default;
            }
            catch
            {
                throw;
            }
        }

        public async Task CreateAsync<TDto>(string uri, TDto dto)
        {
            try
            {
                using StringContent jsonContent = new(
                    JsonSerializer.Serialize(dto),
                    Encoding.UTF8,
                    "application/json");

                using HttpResponseMessage response = await _http.Client.PostAsync(uri, jsonContent);

                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }
        }

        public async Task EditAsync<TDto>(string uri, TDto dto)
        {
            try
            {
                using StringContent jsonContent = new(
                    JsonSerializer.Serialize(dto),
                    Encoding.UTF8,
                    "application/json");

                using HttpResponseMessage response = await _http.Client.PutAsync(uri, jsonContent); 

                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteAsync<TDto>(string uri)
        {
            try
            {
                using HttpResponseMessage response = await _http.Client.DeleteAsync(uri);

                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }
        }
    }
}

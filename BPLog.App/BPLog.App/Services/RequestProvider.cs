using BPLog.App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BPLog.App.Services
{
    public interface IRequestProvider
    {
        Task<RestResponse<TResult>> GetAsync<TResult>(string endpoint);
        Task<RestResponse> DeleteAsync(string endpoint);
        Task<RestResponse> PostAsync(string endpoint, object data);
        Task<RestResponse<TResult>> PostAsync<TResult>(string endpoint, object data);
    }

    public class RequestProvider : IRequestProvider
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializer _serializer = new JsonSerializer();

        public RequestProvider()
        {
            var httpHandler = new HttpClientHandler();
            _httpClient = new HttpClient(httpHandler) { Timeout = TimeSpan.FromMilliseconds(10000) };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<RestResponse<TResult>> GetAsync<TResult>(string endpoint) => await SendRequest<TResult>(new HttpRequestMessage(HttpMethod.Get, new Uri(endpoint)));

        public async Task<RestResponse> DeleteAsync(string endpoint) => await SendRequest(new HttpRequestMessage(HttpMethod.Delete, new Uri(endpoint)));

        public async Task<RestResponse> PostAsync(string endpoint, object data)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(endpoint));
            if (data != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            }
            return await SendRequest(request);
        }

        public async Task<RestResponse<TResult>> PostAsync<TResult>(string endpoint, object data)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(endpoint));
            if (data != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            }

            return await SendRequest<TResult>(request);
        }

        private async Task<RestResponse<TResult>> SendRequest<TResult>(HttpRequestMessage request)
        {
            if (!string.IsNullOrWhiteSpace(Settings.Token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);
            }

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        using (JsonTextReader json = new JsonTextReader(reader))
                        {
                            var result = _serializer.Deserialize<TResult>(json);
                            return RestResponse<TResult>.OkResult(result);
                        }
                    }
                }
            }
            return RestResponse<TResult>.ErrorResult(response.StatusCode);
        }

        private async Task<RestResponse> SendRequest(HttpRequestMessage request)
        {
            if (!string.IsNullOrWhiteSpace(Settings.Token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);
            }

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return new RestResponse { Success = true, StatusCode = HttpStatusCode.OK };
            }
            return new RestResponse { StatusCode = response.StatusCode };
        }
    }
}

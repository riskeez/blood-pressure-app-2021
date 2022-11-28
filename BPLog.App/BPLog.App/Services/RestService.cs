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
    public interface IRestService
    {
        Task<string> Login(string login, string password);
        Task<bool> RegisterUser(string login, string password);
        Task<BloodPressure> GetLastBloodPressure();
        Task<bool> SaveBloodPressure(BloodPressure record);
        Task<bool> DeleteBloodPressure(int id);
        Task<BloodPressurePage> GetBloodPressures(int pageSize, int page, string sort);
    }

    public class RestService : IRestService
    {
        private static string _baseEndpoint = Settings.URL;
        private static string _endpointBloodPressure = $"{_baseEndpoint}/bloodpressure";
        private static string _endpointAuth = $"{_baseEndpoint}/auth";

        private readonly IRequestProvider _requestClient;

        public RestService(IRequestProvider requestProvider)
        {
            _requestClient = requestProvider ?? throw new ArgumentException(nameof(requestProvider));
        }

        public Task<BloodPressurePage> GetBloodPressures(int pageSize, int page, string sort)
        {
            throw new NotImplementedException();
        }

        public async Task<BloodPressure> GetLastBloodPressure()
        {
            var response = await _requestClient.GetAsync<BloodPressure>(_endpointBloodPressure);
            return response.Data;
        }

        public async Task<string> Login(string login, string password)
        {
            var payload = new LoginRequest { Login = login, Password = password };
            var response = await _requestClient.PostAsync<string>(_endpointAuth, payload);
            return response.Data;
        }

        public async Task<bool> RegisterUser(string login, string password)
        {
            var payload = new LoginRequest { Login = login, Password = password };
            var response = await _requestClient.PostAsync($"{_endpointAuth}/register", payload);
            return response.Success;
        }

        public async Task<bool> SaveBloodPressure(BloodPressure record)
        {
            var response = await _requestClient.PostAsync(_endpointBloodPressure, record);
            return response.Success;
        }

        public async Task<bool> DeleteBloodPressure(int id)
        {
            var response = await _requestClient.DeleteAsync($"{_endpointBloodPressure}/id");
            return response.Success;
        }
    }
}

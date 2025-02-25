using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using TaskManagementAPI;

namespace TaskManagementAPI.Tests
{
    public class ApiTestBase
    {
        protected readonly HttpClient _client;
        protected readonly string _baseUrl = "https://localhost"; // Change as needed to your API base URL

        public ApiTestBase()
        {
            // Set up a WebApplicationFactory for your API
            var factory = new WebApplicationFactory<TaskManagementAPI.Program>();  // Startup class of your project
            _client = factory.CreateClient();
        }

        protected async Task<string> GetAuthToken(string username, string password)
        {
            // Adjust the URL according to your API login endpoint
            var loginUrl = $"{_baseUrl}/api/auth/login";

            var loginRequest = new
            {
                Username = username,
                Password = password
            };

            var loginResponse = await _client.PostAsync(loginUrl, new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json"));
            loginResponse.EnsureSuccessStatusCode();

            var responseString = await loginResponse.Content.ReadAsStringAsync();
            var authToken = JsonConvert.DeserializeObject<dynamic>(responseString).token;  // Assuming response has a 'token' field
            return authToken;
        }
    }
}

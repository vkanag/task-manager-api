
using System.Text;
using Newtonsoft.Json;
using System.Net; 

namespace TaskManagementAPI.Tests
{
    public class TaskManagementApiTests:ApiTestBase
    {
        private async Task SetAuthorizationHeaderAsync(string username, string password)
        {
            var authToken = await GetAuthToken(username, password);
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        }

        [Fact]
        public async Task GetAllTasks_ReturnsOk_WhenAuthenticated()
        {
            // Arrange
            await SetAuthorizationHeaderAsync("username", "password");

            // Act
            var response = await _client.GetAsync($"{_baseUrl}/api/task");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Id", responseString);  // Adjust according to your task data format
        }

        [Fact]
        public async Task CreateTask_ReturnsCreated_WhenAuthenticated()
        {
            // Arrange
            await SetAuthorizationHeaderAsync("username", "password");

            var taskRequest = new
            {
                Title = "Task1",
                Description = "Description of the task",
                Priority = 1,
                Deadline = DateTime.Now,
                IsCompleted = true,
                UserId =1
            };

            var content = new StringContent(JsonConvert.SerializeObject(taskRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"{_baseUrl}/api/task", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Id", responseString);  // Adjust according to your task response format
        }

        [Fact]
        public async Task UpdateTask_ReturnsNoContent_WhenAuthenticated()
        {
            // Arrange
            await SetAuthorizationHeaderAsync("username", "password");

            var id = 1;  // Assuming we want to update task with Id 1
            var updatedTaskRequest = new
            {
                Id = id,
                Title = "Updated Task Title",
                Description = "Updated Description of the task",
                Priority = 2,
                Deadline = DateTime.Now.AddDays(1),
                IsCompleted = false,
                UserId = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(updatedTaskRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"{_baseUrl}/api/task/{id}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);  // Assuming PUT returns NoContent (204)
        }

        [Fact]
        public async Task DeleteTask_ReturnsNoContent_WhenAuthenticated()
        {
            // Arrange
            await SetAuthorizationHeaderAsync("username", "password");

            var Id = 1;  // Assuming we want to delete task with Id 1

            // Act
            var response = await _client.DeleteAsync($"{_baseUrl}/api/task/{Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);  // Assuming DELETE returns NoContent (204)
        }


        // User

        [Fact]
        public async Task GetAllUsers_ReturnsOk_WhenAuthenticated()
        {
            // Arrange
            await SetAuthorizationHeaderAsync("username", "password");

            // Act
            var response = await _client.GetAsync($"{_baseUrl}/api/user");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("id", responseString);  // Adjust according to your task data format
        }

        [Fact]
        public async Task CreateUser_ReturnsCreated_WhenAuthenticated()
        {
            // Arrange
            await SetAuthorizationHeaderAsync("username", "password");

            var taskRequest = new
            {
                Name = "ABC",
                Email = "ABC@gmail.com"
            };

            var content = new StringContent(JsonConvert.SerializeObject(taskRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"{_baseUrl}/api/user", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("id", responseString);  // Adjust according to your task response format
        }

        [Fact]
        public async Task UpdateUser_ReturnsNoContent_WhenAuthenticated()
        {
            // Arrange
            await SetAuthorizationHeaderAsync("username", "password");

            var id = 1;  // Assuming we want to update task with Id 1
            var updatedTaskRequest = new
            {
                Id = id,
                Name = "ABC",
                Email = "ABC@gmail.com"
            };

            var content = new StringContent(JsonConvert.SerializeObject(updatedTaskRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"{_baseUrl}/api/user/{id}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);  // Assuming PUT returns NoContent (204)
        }

        [Fact]
        public async Task DeleteUser_ReturnsNoContent_WhenAuthenticated()
        {
            // Arrange
            await SetAuthorizationHeaderAsync("username", "password");

            var Id = 1;  // Assuming we want to delete task with Id 1

            // Act
            var response = await _client.DeleteAsync($"{_baseUrl}/api/user/{Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);  // Assuming DELETE returns NoContent (204)
        }
    }
}

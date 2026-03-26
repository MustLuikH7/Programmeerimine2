using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KooliProjekt.WindowsForms.Api
{
    public class ApiClient : IApiClient
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public ApiClient()
        {
            // The url port could vary, let's use 5086 as in the example or check WebAPI/Properties/launchSettings.json
            _baseUrl = "http://localhost:5086/api/Users/";
            _client = new HttpClient();
        }

        public async Task<OperationResult<PagedResult<UserDto>>> List(int page, int pageSize)
        {
            var url = _baseUrl + "?page=" + page + "&pageSize=" + pageSize;
            return await _client.GetFromJsonAsync<OperationResult<PagedResult<UserDto>>>(url);
        }

        public async Task<OperationResult> Save(SaveUserCommand command)
        {
            var url = _baseUrl + "Save";
            var response = await _client.PostAsJsonAsync(url, command);
            return await response.Content.ReadFromJsonAsync<OperationResult>();
        }

        public async Task<OperationResult> Delete(int userId)
        {
            var url = _baseUrl + "Delete";
            var command = new DeleteUserCommand { UserId = userId };

            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };
            var response = await _client.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<OperationResult>();
        }
    }
}

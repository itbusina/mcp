using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace itbusina.sonar.RestClients
{
    public class ClientBase : HttpClient
    {
        protected ClientBase()
        {
            _jsonOptions.Converters.Add(new JsonStringEnumConverter());
        }

        private JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public void AcceptJson()
        {
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetString(string requestUri)
        {
            var response = await GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(@$"Get API request failed.\n
                Url: {requestUri}\n
                Status code: {response.StatusCode}\n
                Reason: {response.ReasonPhrase}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        public async Task<T?> GetJson<T>(string requestUri)
        {
            var content = await GetString(requestUri);
            var responseObject = JsonSerializer.Deserialize<T>(content, _jsonOptions);
            return responseObject;
        }

        public async Task<JsonDocument> GetJsonDocument(string requestUri)
        {
            var content = await GetString(requestUri);
            var responseObject = JsonDocument.Parse(content);
            return responseObject;
        }

        public async Task<T?> PostJson<T>(string requestUri, object body)
        {
            var response = await this.PostAsJsonAsync(requestUri, body);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(@$"Post API request failed.\n
                Url: {requestUri}\n
                Body: {JsonSerializer.Serialize(body)}\n
                Status code: {response.StatusCode}\n
                Reason: {response.ReasonPhrase}");
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<T>(responseData, _jsonOptions);
            return responseObject;
        }
    }
}
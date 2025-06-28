using System.Net.Http.Headers;

namespace itbusina.sonar.RestClients
{
    public class AuthenticatedClient : ClientBase
    {
        public void AddBearerAuthentication(string token)
        {
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void AddBasicAuthentication(string username, string token)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes($"{username}:{token}");
            var base64String = Convert.ToBase64String(bytes);
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
        }
    }
}
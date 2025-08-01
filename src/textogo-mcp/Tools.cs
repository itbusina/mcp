using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace textogo_mcp
{
    [McpServerToolType]
    public static class Tools
    {
        [McpServerTool, Description("Converts text to audio using Textogo service.")]
        public static async Task<string> Convert(
            HttpClient client,
            ILogger<Tool> logger,
            [Description("The text to convert to audio"), Required] string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentOutOfRangeException(nameof(text), "Text must be present");

            var billingId = Environment.GetEnvironmentVariable("BILLING_ID");

            var response = await client.PostAsync($"api/mcp/tts?billingId={billingId}", new StringContent(text));
            var audioUrl = await response.Content.ReadAsStringAsync();
            return audioUrl;
        }
    }
}
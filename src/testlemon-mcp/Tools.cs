
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace testlemon_mcp
{
    [McpServerToolType, Description("Tools to access Testlemon REST API.")]
    public static class Tools
    {
        [McpServerTool, Description("Validates the given domain for DNS, DKIM and other settings.")]
        public static async Task<string> ValidateDomain(
            HttpClient client,
            ILogger<Tool> logger,
            [Description("Domain url address to validate and check.")] string domain)
        {
            var result = await client.GetStringAsync($"/api/domain?q={domain}");
            return result;
        }
    }
}
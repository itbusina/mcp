
using itbusina.sonar;
using common;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace sonar
{
    [McpServerToolType]
    public static class Tools
    {
        [McpServerTool, Description("Search for the authenticated user favorites. Requires authentication.")]
        public static async Task<string> GetFavorites(
            SonarClient client,
            ILogger<Tool> logger,
            [Description("1-based page number. Default value: 1. Example value: 42. Optional.")] int? page = 1,
            [Description("Page size. Must be greater than 0 and less or equal than 500. Default: 100. Maximum: 500. Example: 20. Optional.")] int? pageSize = 100)
        {
            page ??= 1;
            pageSize ??= 100;

            return await logger.ExecuteAndLog(nameof(GetFavorites), async () =>
            {
                var favorites = await client.GetFavoritesAsync(page, pageSize);
                return JsonSerializer.Serialize(favorites);
            });
        }
    }
}
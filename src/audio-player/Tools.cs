using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Diagnostics;

namespace audio_player
{
    [McpServerToolType]
    public static class Tools
    {
        private readonly static HttpClient _httpClient = new();

        [McpServerTool, Description("Plays audio from remote url")]
        public static async Task<string> Play(
            ILogger<Tool> logger,
            [Description("The url to audio file")] string audioUrl)
        {
            if (string.IsNullOrWhiteSpace(audioUrl))
                throw new ArgumentOutOfRangeException(nameof(audioUrl), "url must be present");

            var tempFile = Path.Combine(Path.GetTempPath(), "temp-audio.mp3");

            using (var httpClient = new HttpClient())
            {
                var bytes = await httpClient.GetByteArrayAsync(audioUrl);
                await File.WriteAllBytesAsync(tempFile, bytes);
            }

            var ffplayPath = Path.Combine(AppContext.BaseDirectory, "player", "ffplay");
            var startInfo = new ProcessStartInfo
            {
                FileName = ffplayPath,
                Arguments = $"-nodisp -autoexit -loglevel quiet \"{tempFile}\"",
                UseShellExecute = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false
            };

            using var process = Process.Start(startInfo);
            await process.WaitForExitAsync();

            return "Completed";
        }
    }
}
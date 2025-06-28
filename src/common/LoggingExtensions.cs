using Microsoft.Extensions.Logging;

namespace common
{
    public static class LoggingExtensions
    {
        public static async Task<T> ExecuteAndLog<T>(
            this ILogger logger, string methodName, Func<Task<T>> action)
        {
            try
            {
                logger.LogInformation("Starting {MethodName}", methodName);
                var result = await action();
                logger.LogInformation("Completed {MethodName}. Result: {Result}", methodName, result);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }
    }
}
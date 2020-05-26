using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Swisschain.Sdk.Server.Authorization;

namespace Assets.Exceptions
{
    public static class HttpExtensions
    {
        public static async Task<string> GetBodyAsync(this HttpRequest request)
        {
            if (request.Method != "POST")
                return null;

            request.EnableBuffering();
            string body;

            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                body = await reader.ReadToEndAsync();
            }

            request.Body.Seek(0, SeekOrigin.Begin);

            return body;
        }

        public static ILogger GetEnrichLogger(this HttpContext context, string body)
        {
            var brokerId = context.User.GetTenantId();

            var logger = Log
                .ForContext("BrokerId", brokerId);

            if (!string.IsNullOrWhiteSpace(body))
                logger = logger.ForContext("RequestBody", body);

            return logger;
        }
    }
}

using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;

namespace Amazon.Lambda.Hosting.Handlers
{
    public abstract class BaseOutputHandler<TOutput> : ILambdaHandler
    {
        private static readonly MemoryStream ResponseStream = new MemoryStream(0);
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async Task<InvocationResponse> HandleAsync(InvocationRequest invocation)
        {
            var context = invocation.LambdaContext;

            var response = await HandleAsync(context);

            ResponseStream.SetLength(0);
            await JsonSerializer.SerializeAsync(ResponseStream, response, JsonOptions);
            ResponseStream.Position = 0;
            return new InvocationResponse(ResponseStream, false);
        }

        protected abstract Task<TOutput> HandleAsync(ILambdaContext context);
    }
}
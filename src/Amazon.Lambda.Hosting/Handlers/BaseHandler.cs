using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;

namespace Amazon.Lambda.Hosting.Handlers
{
    public abstract class BaseHandler<TRequest, TResponse> : ILambdaHandler
    {
        private static readonly MemoryStream ResponseStream = new MemoryStream(0);
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async Task<InvocationResponse> HandleAsync(InvocationRequest invocation)
        {
            var request = await JsonSerializer.DeserializeAsync<TRequest>(invocation.InputStream);
            var context = invocation.LambdaContext;

            var response = await HandleAsync(request, context);

            ResponseStream.SetLength(0);
            await JsonSerializer.SerializeAsync(ResponseStream, response, JsonOptions);
            ResponseStream.Position = 0;
            return new InvocationResponse(ResponseStream, false);
        }

        protected abstract Task<TResponse> HandleAsync(TRequest request, ILambdaContext context);
    }
}
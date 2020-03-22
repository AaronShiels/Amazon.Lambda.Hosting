using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;

namespace Amazon.Lambda.Hosting.Handlers
{
    public abstract class BaseInputHandler<TRequest> : ILambdaHandler
    {
        private static readonly MemoryStream ResponseStream = new MemoryStream(0);

        public async Task<InvocationResponse> HandleAsync(InvocationRequest invocation)
        {
            var request = await JsonSerializer.DeserializeAsync<TRequest>(invocation.InputStream);
            var context = invocation.LambdaContext;

            await HandleAsync(request, context);

            return new InvocationResponse(ResponseStream, false);
        }

        protected abstract Task HandleAsync(TRequest request, ILambdaContext context);
    }
}
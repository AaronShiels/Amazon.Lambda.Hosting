using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;

namespace Amazon.Lambda.Hosting.Functions
{
    public abstract class BaseInputFunction<TRequest> : ILambdaFunction
    {
        private static readonly MemoryStream ResponseStream = new MemoryStream(0);

        public async Task<InvocationResponse> InvokeAsync(InvocationRequest invocation)
        {
            var request = await JsonSerializer.DeserializeAsync<TRequest>(invocation.InputStream);
            var context = invocation.LambdaContext;

            await InvokeAsync(request, context);

            return new InvocationResponse(ResponseStream, false);
        }

        protected abstract Task InvokeAsync(TRequest request, ILambdaContext context);
    }
}
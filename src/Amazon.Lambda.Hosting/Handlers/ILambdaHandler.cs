using System.Threading.Tasks;
using Amazon.Lambda.RuntimeSupport;

namespace Amazon.Lambda.Hosting.Handlers
{
    public interface ILambdaHandler
    {
        Task<InvocationResponse> HandleAsync(InvocationRequest invocation);
    }
}
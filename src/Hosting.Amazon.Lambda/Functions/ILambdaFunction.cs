using System.Threading.Tasks;
using Amazon.Lambda.RuntimeSupport;

namespace Hosting.Amazon.Lambda.Functions
{
    public interface ILambdaFunction
    {
        Task<InvocationResponse> InvokeAsync(InvocationRequest invocation);
    }
}
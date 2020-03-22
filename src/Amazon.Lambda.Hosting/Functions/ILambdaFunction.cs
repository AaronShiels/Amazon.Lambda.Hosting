using System.Threading.Tasks;
using Amazon.Lambda.RuntimeSupport;

namespace Amazon.Lambda.Hosting.Functions
{
    public interface ILambdaFunction
    {
        Task<InvocationResponse> InvokeAsync(InvocationRequest invocation);
    }
}
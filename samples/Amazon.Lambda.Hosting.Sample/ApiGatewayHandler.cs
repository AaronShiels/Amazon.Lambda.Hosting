using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Hosting.Handlers;
using Microsoft.Extensions.Logging;

namespace Amazon.Lambda.Hosting.Sample
{
    public class ApiGatewayHandler : BaseHandler<APIGatewayProxyRequest, APIGatewayProxyResponse>
    {
        private readonly ILogger<ApiGatewayHandler> _log;

        public ApiGatewayHandler(ILogger<ApiGatewayHandler> log)
        {
            _log = log;
        }

        protected override Task<APIGatewayProxyResponse> HandleAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            _log.LogInformation("Invoked request!");

            var response = new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = "Hello world!"
            };
            return Task.FromResult(response);
        }
    }
}
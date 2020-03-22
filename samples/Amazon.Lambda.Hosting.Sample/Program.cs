using System.Threading.Tasks;
using Amazon.Lambda.Hosting.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Amazon.Lambda.Hosting.Sample
{
    public class Program
    {
        public static Task Main(string[] args) => CreateHostBuilder(args).Build().StartAsync();

        public static IHostBuilder CreateHostBuilder(string[] args) => LambdaHost
            .CreateDefaultBuilder()
            .ConfigureLogging((ctx, logging) => logging.AddConsole())
            .ConfigureServices(services => services.AddSingleton<ILambdaHandler, ApiGatewayHandler>());
    }
}
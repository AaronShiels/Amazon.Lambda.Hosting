using System.Threading.Tasks;
using Hosting.Amazon.Lambda.Functions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hosting.Amazon.Lambda.Sample
{
    public class Program
    {
        public static Task Main(string[] args) => CreateHostBuilder(args).Build().StartAsync();

        public static IHostBuilder CreateHostBuilder(string[] args) => LambdaHost
            .CreateDefaultBuilder()
            .ConfigureLogging((ctx, logging) => logging.AddConsole())
            .ConfigureServices(services => services.AddSingleton<ILambdaFunction, ApiGatewayFunction>());
    }
}
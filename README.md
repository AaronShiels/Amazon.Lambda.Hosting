# Amazon.Lambda.Hosting ![publish](https://github.com/AaronShiels/Amazon.Lambda.Hosting/workflows/publish/badge.svg) [![nuget](https://img.shields.io/nuget/v/Amazon.Lambda.Hosting.svg)](https://www.nuget.org/packages/Amazon.Lambda.Hosting/)
An IHost implementation of [Amazon.Lambda.RuntimeSupport](https://github.com/aws/aws-lambda-dotnet/tree/master/Libraries/src/Amazon.Lambda.RuntimeSupport/), allowing for first-class support for logging, configuration and dependency-injection and .NET Core 3.0+ compatibility. Based on the AWS Blog post [.NET Core 3.0 on Lambda with AWS Lambda’s Custom Runtime](https://aws.amazon.com/blogs/developer/net-core-3-0-on-lambda-with-aws-lambdas-custom-runtime/).

## Features
* .NET Core 3.0+ support.
* Familiar host builder API with implementations for `IHost` and `IHostBuilder`.
* Dependency-injection support, including service lifetimes.
* Default JSON-based implementations using [System.Text.Json](https://github.com/dotnet/runtime/tree/master/src/libraries/System.Text.Json).

## Usage
Create a new console library project and install [Amazon.Lambda.Hosting](https://www.nuget.org/packages/Amazon.Lambda.Hosting/).
```
dotnet new console
dotnet add package Amazon.Lambda.Hosting
```

Create an implementation of `ILambdaFunction` to house the logic of your Lambda function. Provided in this library are 3 useful base classes that encapsulate the JSON serialization fundamentals:
* `BaseFunction` - Request/response.
* `BaseInputFunction` - Requests only (empty response).
* `BaseOutputFunction` - Response only (empty request).
```c#
public class ApiGatewayFunction : BaseFunction<APIGatewayProxyRequest, APIGatewayProxyResponse>
{
    private readonly ILogger<ApiGatewayFunction> _log;

    public ApiGatewayFunction(ILogger<ApiGatewayFunction> log)
    {
        _log = log;
    }

    protected async override Task<APIGatewayProxyResponse> InvokeAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _log.LogInformation("Invoked request!");

        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
            Body = "Hello world!"
        };
    }
}
```
>Note: [Amazon.Lambda.APIGatewayEvents](https://github.com/aws/aws-lambda-dotnet/tree/master/Libraries/src/Amazon.Lambda.APIGatewayEvents/) NuGet package houses the common classes used by AWS's API Gateway Proxy.

Create a `LambdaHost` in Program.cs like you would any other AspNetCore or WorkerService application.
```c#
public class Program
{
    public static Task Main(string[] args) => CreateHostBuilder(args).Build().StartAsync();

    public static IHostBuilder CreateHostBuilder(string[] args) => LambdaHost
        .CreateDefaultBuilder()
        .ConfigureLogging((ctx, logging) => logging.AddConsole())
        .ConfigureServices(services => services.AddSingleton<ILambdaFunction, ApiGatewayFunction>());
}
```

Set the application's assembly name to "bootstrap" in your .csproj file.
```xml
<PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <AssemblyName>bootstrap</AssemblyName>
</PropertyGroup>
```
>Note: AWS executes ./bootstrap by convention. Normally this file is generated during packaging and passes requests through to the C# function we provide. But in this case, we are providing the full host, thus we must compile a stand-along executable called "bootstrap".

Install the [AWS .NET CLI Tools](https://github.com/aws/aws-extensions-for-dotnet-cli)
```
dotnet tool install -g Amazon.Lambda.Tools
```

Package your lambda as a self-contained application.
```
dotnet lambda package -c Release --msbuild-parameters "--self-contained true"
```

Deploy or invoke your function using the [Serverless Framework](https://serverless.com/framework/docs/providers/aws/cli-reference/) CLI tools or equivalent AWS tools. Provided is a [sample project](./samples/Amazon.Lambda.Hosting.Sample) with a valid [serverless.yml](./samples/Amazon.Lambda.Hosting.Sample/serverless.yml).
```
serverless invoke local --function sample --data "{}"
serverless deploy --verbose
```
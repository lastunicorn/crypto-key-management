using AsyncMediator.Extensions.DependencyInjection;
using DustInTheWind.SignatureManagement.Application.CreateSignature;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.UserAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DustInTheWind.SignatureManagement;

internal static class Program
{
    private static Task Main(string[] args)
    {
        Console.WriteLine("Ed25519 Signature Management Tool");
        Console.WriteLine("=================================\n");

        return Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddHostedService<HostedService>();
                
                services.AddTransient<ISignatureRepository, SignatureRepository>();
                
                services.AddSingleton<CommandLoop>();
                services.AddScoped<IUserConsole, UserConsole>();

                services.AddAsyncMediator(typeof(CreateSignatureCommand).Assembly);
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            })
            .RunConsoleAsync();
    }
}

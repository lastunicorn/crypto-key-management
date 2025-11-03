using System.Reflection;
using AsyncMediator.Extensions.DependencyInjection;
using DustInTheWind.CryptoKeyManagement.Application.CreateKeyPair;
using DustInTheWind.CryptoKeyManagement.Ports.SignatureAccess;
using DustInTheWind.CryptoKeyManagement.Ports.UserAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DustInTheWind.CryptoKeyManagement;

internal static class Program
{
    private static Task Main(string[] args)
    {
        Version version = Assembly.GetEntryAssembly().GetName().Version ?? new Version(0, 0, 0, 0);

        Console.WriteLine("Signature Management Tool - " + version.ToString(3));
        Console.WriteLine("=================================\n");

        return Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddHostedService<HostedService>();
                
                services.AddTransient<ISignatureKeyRepository, SignatureKeyRepository>();
                
                services.AddSingleton<CommandLoop>();
                services.AddScoped<IUserConsole, UserConsole>();

                services.AddAsyncMediator(typeof(CreateKeyPairRequest).Assembly);
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            })
            .RunConsoleAsync();
    }
}
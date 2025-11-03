using Microsoft.Extensions.Hosting;

namespace DustInTheWind.CryptoKeyManagement;

internal sealed class HostedService : IHostedService
{
    private readonly IHostApplicationLifetime hostApplicationLifetime;
    private readonly CommandLoop commandLoop;

    public HostedService(IHostApplicationLifetime hostApplicationLifetime, CommandLoop commandLoop)
    {
        this.hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
        this.commandLoop = commandLoop ?? throw new ArgumentNullException(nameof(commandLoop));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        hostApplicationLifetime.ApplicationStarted
            .Register(() =>
            {
                commandLoop.Closed += (o, e) =>
                {
                    hostApplicationLifetime.StopApplication();
                };

                _ = commandLoop.RunAsync();

            });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

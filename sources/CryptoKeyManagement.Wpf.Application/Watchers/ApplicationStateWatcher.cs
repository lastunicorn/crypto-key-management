using System.ComponentModel;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentMain;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.Watchers;

public class ApplicationStateWatcher
{
    private readonly IApplicationState applicationState;
    private readonly IEventBus eventBus;

    public ApplicationStateWatcher(IApplicationState applicationState, IEventBus eventBus)
    {
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public void Start()
    {
        applicationState.PropertyChanged += HandlePropertyChanged;
    }

    public void Stop()
    {
        applicationState.PropertyChanged -= HandlePropertyChanged;
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IApplicationState.CurrentSignatureKey))
        {
            KeyPairSelectionChangedEvent @event = new()
            {
                SignatureKey = applicationState.CurrentSignatureKey?.ToDto()
            };

            _ = eventBus.PublishAsync(@event);
        }
    }
}

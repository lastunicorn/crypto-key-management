using System.ComponentModel;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;

namespace DustInTheWind.SignatureManagement.Wpf.Application.Watchers;

public class ApplicationStateWatcher
{
    private readonly IApplicationState applicationState;
    private readonly EventBus eventBus;

    public ApplicationStateWatcher(IApplicationState applicationState, EventBus eventBus)
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

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
        applicationState.CurrentSignatureKeyChanged += HandleSelectedSignatureKeyChanged;
    }

    public void Stop()
    {
        applicationState.CurrentSignatureKeyChanged -= HandleSelectedSignatureKeyChanged;
    }

    private void HandleSelectedSignatureKeyChanged(object sender, CurrentSignatureKeyChangedEventArgs e)
    {
        SignatureKeySelectionChangedEvent @event = new()
        {
            SignatureKey = e.SignatureKey.ToDto()
        };

        _ = eventBus.PublishAsync(@event);
    }
}

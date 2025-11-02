
namespace DustInTheWind.SignatureManagement.Infrastructure;

public interface IEventBus
{
    Task PublishAsync<TEvent>(CancellationToken cancellationToken = default);

    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default);

    void Subscribe<TEvent>(Func<TEvent, CancellationToken, Task> action);

    void Unsubscribe<TEvent>(Func<TEvent, CancellationToken, Task> action);

    void UnsubscribeAllForMe();
}
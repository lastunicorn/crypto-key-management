namespace DustInTheWind.SignatureManagement.Infrastructure;

public class EventBus
{
    private readonly SubscribersCollection subscribersByEvent = new();

    public void Subscribe<TEvent>(Func<TEvent, CancellationToken, Task> action)
    {
        List<object> actions = subscribersByEvent.GetOrCreateBucket<TEvent>();
        actions.Add(action);
    }

    public void Unsubscribe<TEvent>(Func<TEvent, CancellationToken, Task> action)
    {
        List<object> actions = subscribersByEvent.GetBucket<TEvent>();
        actions?.Remove(action);
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
        List<object> bucket = subscribersByEvent.GetBucket<TEvent>();

        if (bucket == null)
            return;

        IEnumerable<Func<TEvent, CancellationToken, Task>> actions = bucket.Cast<Func<TEvent, CancellationToken, Task>>();

        foreach (Func<TEvent, CancellationToken, Task> action in actions)
            await action(@event, cancellationToken);
    }

    public Task PublishAsync<TEvent>(CancellationToken cancellationToken = default)
    {
        TEvent @event = Activator.CreateInstance<TEvent>();
        return PublishAsync(@event, cancellationToken);
    }
}

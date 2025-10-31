using System.Diagnostics;
using System.Reflection;

namespace DustInTheWind.SignatureManagement.Infrastructure;

public class EventBus
{
    private readonly SubscriptionCollection subscriptionsByEvent = new();

    public void Subscribe<TEvent>(Func<TEvent, CancellationToken, Task> action)
    {
        Type subscriberType = GetCallerType();

        List<Subscription> subscriptions = subscriptionsByEvent.GetOrCreateBucket<TEvent>();
        Subscription subscription = new(action, subscriberType);
        subscriptions.Add(subscription);
    }

    public void Unsubscribe<TEvent>(Func<TEvent, CancellationToken, Task> action)
    {
        List<Subscription> subscriptions = subscriptionsByEvent.GetBucket<TEvent>();

        if (subscriptions != null)
        {
            Subscription subscriptionToRemove = subscriptions
                .FirstOrDefault(x => ReferenceEquals(x.Action, action));

            if (subscriptionToRemove != null)
                subscriptions.Remove(subscriptionToRemove);
        }
    }

    public void UnsubscribeAllForMe()
    {
        // Get the type of the calling class using stack trace
        Type callerType = GetCallerType();

        if (callerType == null)
            return;

        // Remove all subscriptions for this caller type across all events
        foreach (List<Subscription> subscriptions in subscriptionsByEvent.GetAllBuckets())
        {
            subscriptions.RemoveAll(x => x.SubscriberType == callerType);
        }
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
        List<Subscription> subscriptions = subscriptionsByEvent.GetBucket<TEvent>();

        if (subscriptions == null)
            return;

        IEnumerable<Func<TEvent, CancellationToken, Task>> actions = subscriptions
            .Select(x => x.Action)
            .Cast<Func<TEvent, CancellationToken, Task>>();

        foreach (Func<TEvent, CancellationToken, Task> action in actions)
            await action(@event, cancellationToken);
    }

    public Task PublishAsync<TEvent>(CancellationToken cancellationToken = default)
    {
        TEvent @event = Activator.CreateInstance<TEvent>();
        return PublishAsync(@event, cancellationToken);
    }

    private Type GetCallerType()
    {
        // Get the stack trace to find the calling method
        StackTrace stackTrace = new StackTrace();

        // Skip the current method (GetCallerType) and UnsubscribeAllForMe
        for (int i = 2; i < stackTrace.FrameCount; i++)
        {
            StackFrame frame = stackTrace.GetFrame(i);
            MethodBase method = frame?.GetMethod();

            if (method != null && method.DeclaringType != typeof(EventBus))
            {
                return method.DeclaringType;
            }
        }

        return null;
    }
}

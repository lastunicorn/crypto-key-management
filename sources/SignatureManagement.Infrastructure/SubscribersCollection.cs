namespace DustInTheWind.SignatureManagement.Infrastructure;

/// <summary>
/// A collection of subscriber objects grouped by event type.
/// </summary>
internal class SubscribersCollection
{
    private readonly Dictionary<Type, List<object>> subscribersByEvent = new();

    public List<object> GetOrCreateBucket<TEvent>()
    {
        return GetBucket<TEvent>() ?? CreateBucket<TEvent>();
    }

    public List<object> GetBucket<TEvent>()
    {
        return subscribersByEvent.ContainsKey(typeof(TEvent))
            ? subscribersByEvent[typeof(TEvent)]
            : null;
    }

    public List<object> CreateBucket<TEvent>()
    {
        List<object> actions = new();
        subscribersByEvent.Add(typeof(TEvent), actions);
        return actions;
    }
}
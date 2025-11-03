namespace DustInTheWind.CryptoKeyManagement.Infrastructure;

/// <summary>
/// A collection of subscriber objects grouped by event type.
/// </summary>
internal class SubscriptionCollection
{
    private readonly Dictionary<Type, List<Subscription>> subscriptionsByEvent = [];

    public List<Subscription> GetOrCreateBucket<TEvent>()
    {
        return GetBucket<TEvent>() ?? CreateBucket<TEvent>();
    }

    public List<Subscription> GetBucket<TEvent>()
    {
        return subscriptionsByEvent.ContainsKey(typeof(TEvent))
            ? subscriptionsByEvent[typeof(TEvent)]
            : null;
    }

    public List<Subscription> CreateBucket<TEvent>()
    {
        List<Subscription> subscribers = [];
        subscriptionsByEvent.Add(typeof(TEvent), subscribers);
        return subscribers;
    }

    public IEnumerable<List<Subscription>> GetAllBuckets()
    {
        return subscriptionsByEvent.Values;
    }
}
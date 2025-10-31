namespace DustInTheWind.SignatureManagement.Infrastructure;

/// <summary>
/// Information about a subscriber including the action and the type of the subscriber.
/// </summary>
internal class Subscription
{
    public object Action { get; set; }

    public Type SubscriberType { get; set; }
    
    public Subscription(object action, Type subscriberType)
    {
        Action = action;
        SubscriberType = subscriberType;
    }
}

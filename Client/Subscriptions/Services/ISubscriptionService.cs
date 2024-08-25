namespace Orion.Client.Subscriptions.Services
{
    public interface ISubscriptionService
    {
        public Subscriptable<T> GetOrCreateSubscriptableForTopic<T>(string topic) where T : Enum;
    }
}

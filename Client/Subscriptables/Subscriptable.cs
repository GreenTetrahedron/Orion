using Orion.Models.ServerTransmissions.Results;

namespace Orion.Client.Subscriptables
{
    public class Subscriptable<T> where T : Enum
    {
        private readonly List<Action<ServerResult<T>>> _subscriptions;

        public Subscriptable()
        {
            _subscriptions = new List<Action<ServerResult<T>>>();
        }

        public void Subscribe(Action<ServerResult<T>> subscription)
        {
            _subscriptions.Add(subscription);
        }
    }
}
using Orion.Models.ServerTransmissions.Results;

namespace Orion.Client.Subscriptions
{
    public class Subscriptable<T> where T : Enum
    {
        private readonly List<Action<ServerResult<T>>> _subscriptions;

        public Subscriptable()
        {
            _subscriptions = new List<Action<ServerResult<T>>>();
        }

        public Subscriptable(List<Action<ServerResult<T>>> subscriptions)
        {
            _subscriptions = subscriptions;
        }

        public void Subscribe(Action<ServerResult<T>> subscription)
        {
            _subscriptions.Add(subscription);
        }

        public void Publish(ServerResult<T> serverResult)
        {
            foreach(var subscription in _subscriptions)
            {
                subscription.Invoke(serverResult);
            }
        }

        public static implicit operator Subscriptable<T>(Subscriptable<Enum> value) =>
            new Subscriptable<T>(value._subscriptions.Select(x => (Action<ServerResult<T>>)x).ToList());

        public static implicit operator Subscriptable<Enum>(Subscriptable<T> value) =>
            new Subscriptable<Enum>(value._subscriptions.Select(x => (Action<ServerResult<Enum>>)x).ToList());
    }
}
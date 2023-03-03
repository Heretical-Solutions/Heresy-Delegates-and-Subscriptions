using System;
using System.Collections.Generic;

namespace HereticalSolutions.Delegates.Broadcasting
{
    public class Broadcaster
        : IPublisherSingleArgument,
          ISubscribableSingleArgument
    {
        #region Subscriptions

        private IReadOnlyRepository<Type, object> broadcasterDatabase;

        #endregion

        #region IPublisherSingleArgument

        public void Publish<TValue>(TValue value)
        {
            var messageType = typeof(TValue);
            
            if (!broadcasterDatabase.TryGetValue(
                    messageType,
                    out object broadcasterObject))
                return;

            var broadcaster = (IPublisherSingleArgumentGeneric<TValue>)broadcasterObject;
            
            broadcaster.Publish(value);
        }

        #endregion

        #region ISubscribableSingleArgumentGeneric
		
        public void Subscribe(object @delegate)
        {
            multicastDelegate += Handle(@delegate);
        }

        public void Unsubscribe<TArgument>(Action<TArgument> @delegate)
        {
            multicastDelegate -= Handle(@delegate);
        }

        private void Handle<TArgument>(Action<TArgument> @delegate, object value)
        {
            @delegate?.Invoke((TArgument)value);
        }

        #endregion
    }
}
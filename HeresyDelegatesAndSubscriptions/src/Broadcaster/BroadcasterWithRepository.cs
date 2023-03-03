using System;
using HereticalSolutions.Repositories;

namespace HereticalSolutions.Delegates.Broadcasting
{
    public class BroadcasterWithRepository
        : IPublisherSingleArgument,
          ISubscribableSingleArgument
    {
        private IReadOnlyRepository<Type, object> broadcasterRepository;

        public BroadcasterWithRepository(IReadOnlyRepository<Type, object> broadcasterRepository)
        {
            this.broadcasterRepository = broadcasterRepository;
        }

        #region IPublisherSingleArgument

        public void Publish<TValue>(TValue value)
        {
            var messageType = typeof(TValue);
            
            if (!broadcasterRepository.TryGet(
                    messageType,
                    out object broadcasterObject))
                return;

            var broadcaster = (IPublisherSingleArgumentGeneric<TValue>)broadcasterObject;
            
            broadcaster.Publish(value);
        }

        #endregion

        #region ISubscribableSingleArgumentGeneric
		
        public void Subscribe<TValue>(Action<TValue> @delegate)
        {
            var messageType = typeof(TValue);
            
            if (!broadcasterRepository.TryGet(
                    messageType,
                    out object broadcasterObject))
                return;

            var broadcaster = (ISubscribableSingleArgumentGeneric<TValue>)broadcasterObject;
            
            broadcaster.Subscribe(@delegate);
        }

        public void Unsubscribe<TValue>(Action<TValue> @delegate)
        {
            var messageType = typeof(TValue);
            
            if (!broadcasterRepository.TryGet(
                    messageType,
                    out object broadcasterObject))
                return;

            var broadcaster = (ISubscribableSingleArgumentGeneric<TValue>)broadcasterObject;
            
            broadcaster.Unsubscribe(@delegate);
        }

        #endregion
    }
}
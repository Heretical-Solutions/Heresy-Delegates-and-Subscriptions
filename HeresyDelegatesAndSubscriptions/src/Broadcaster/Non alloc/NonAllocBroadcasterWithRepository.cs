using System;
using HereticalSolutions.Repositories;

namespace HereticalSolutions.Delegates.Broadcasting
{
    public class NonAllocBroadcasterWithRepository
        : IPublisherSingleArg,
          INonAllocSubscribableSingleArg
    {
        private readonly IReadOnlyObjectRepository broadcasterRepository;

        public NonAllocBroadcasterWithRepository(IReadOnlyObjectRepository broadcasterRepository)
        {
            this.broadcasterRepository = broadcasterRepository;
        }

        #region IPublisherSingleArg

        public void Publish<TValue>(TValue value)
        {
            var messageType = typeof(TValue);
            
            if (!broadcasterRepository.TryGet(
                    messageType,
                    out object broadcasterObject))
                return;

            var broadcaster = (IPublisherSingleArgGeneric<TValue>)broadcasterObject;
            
            broadcaster.Publish(value);
        }

        #endregion

        #region INonAllocSubscribableSingleArg
		
        public void Subscribe<TValue>(Action<TValue> @delegate)
        {
            var messageType = typeof(TValue);
            
            if (!broadcasterRepository.TryGet(
                    messageType,
                    out object broadcasterObject))
                return;

            var broadcaster = (ISubscribableSingleArgGeneric<TValue>)broadcasterObject;
            
            broadcaster.Subscribe(@delegate);
        }

        public void Unsubscribe<TValue>(Action<TValue> @delegate)
        {
            var messageType = typeof(TValue);
            
            if (!broadcasterRepository.TryGet(
                    messageType,
                    out object broadcasterObject))
                return;

            var broadcaster = (ISubscribableSingleArgGeneric<TValue>)broadcasterObject;
            
            broadcaster.Unsubscribe(@delegate);
        }

        #endregion

        public void Subscribe(ISubscriptionHandler<INonAllocSubscribableSingleArg, IInvokableSingleArg> subscription)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(ISubscriptionHandler<INonAllocSubscribableSingleArg, IInvokableSingleArg> subscription)
        {
            throw new NotImplementedException();
        }
    }
}
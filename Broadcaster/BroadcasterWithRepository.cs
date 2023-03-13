using System;

using HereticalSolutions.Repositories;

namespace HereticalSolutions.Delegates.Broadcasting
{
    public class BroadcasterWithRepository
        : IPublisherSingleArg,
          ISubscribableSingleArg
    {
        private readonly IReadOnlyObjectRepository broadcasterRepository;

        public BroadcasterWithRepository(IReadOnlyObjectRepository broadcasterRepository)
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
        
        public void Publish(Type valueType, object value)
        {
            var messageType = valueType;
            
            if (!broadcasterRepository.TryGet(
                    messageType,
                    out object broadcasterObject))
                return;

            var broadcaster = (IPublisherSingleArg)broadcasterObject;
            
            broadcaster.Publish(value);
        }

        #endregion

        #region ISubscribableSingleArg
		
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

        public void Subscribe(Type valueType, object @delegate)
        {
            var messageType = valueType;
            
            if (!broadcasterRepository.TryGet(
                    messageType,
                    out object broadcasterObject))
                return;

            var broadcaster = (ISubscribableSingleArg)broadcasterObject;
            
            broadcaster.Subscribe(valueType, @delegate);
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

        public void Unsubscribe(Type valueType, object @delegate)
        {
            var messageType = valueType;
            
            if (!broadcasterRepository.TryGet(
                    messageType,
                    out object broadcasterObject))
                return;

            var broadcaster = (ISubscribableSingleArg)broadcasterObject;
            
            broadcaster.Unsubscribe(valueType, @delegate);
        }

        #endregion
    }
}
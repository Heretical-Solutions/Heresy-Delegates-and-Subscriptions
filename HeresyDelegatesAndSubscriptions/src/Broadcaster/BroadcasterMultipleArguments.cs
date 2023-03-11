using System;

namespace HereticalSolutions.Delegates.Broadcasting
{
    public class BroadcasterMultipleArguments
        : IPublisherMultipleArguments,
          ISubscribableMultipleArguments
    {
        private readonly BroadcasterGeneric<object[]> innerBroadcaster;

        public BroadcasterMultipleArguments(BroadcasterGeneric<object[]> innerBroadcaster)
        {
            this.innerBroadcaster = innerBroadcaster;
        }

        #region IPublisherMultipleArguments

        public void Publish(object[] values)
        {
            innerBroadcaster.Publish(values);
        }

        #endregion

        #region ISubscribableSMultipleArguments
		
        public void Subscribe(Action<object[]> @delegate)
        {
            
            innerBroadcaster.Subscribe(@delegate);
        }

        public void Unsubscribe(Action<object[]> @delegate)
        {
            innerBroadcaster.Unsubscribe(@delegate);
        }

        #endregion
    }
}
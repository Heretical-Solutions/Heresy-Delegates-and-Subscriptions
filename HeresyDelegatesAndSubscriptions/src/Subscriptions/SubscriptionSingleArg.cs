using System;

using HereticalSolutions.Delegates.Factories;

using HereticalSolutions.Pools;

namespace HereticalSolutions.Delegates.Subscriptions
{
    public class SubscriptionSingleArg
        : ISubscription<INonAllocSubscribableSingleArg>,
          ISubscriptionHandler<INonAllocSubscribableSingleArg, IInvokableSingleArg>
    {
        public SubscriptionSingleArg(
            Action<object> @delegate)
        {
            Delegate = DelegatesFactory.BuildDelegateWrapperSingleArg(@delegate);

            Active = false;

            Publisher = null;

            PoolElement = null;
        }

        #region ISubscription
        
        public bool Active { get; private set;  }
        
        public INonAllocSubscribableSingleArg Publisher { get; private set; }

        public void Subscribe(INonAllocSubscribableSingleArg publisher)
        {
            if (Active)
                return;
            
            publisher.Subscribe(this);
        }

        public void Unsubscribe()
        {
            if (!Active)
                return;

            Publisher.Unsubscribe(this);
        }
        
        #endregion

        #region ISubscriptionHandler

        public IInvokableSingleArg Delegate { get; private set; }

        public IPoolElement<IInvokableSingleArg> PoolElement { get; private set; }
        
        public void Activate(
            INonAllocSubscribableSingleArg publisher,
            IPoolElement<IInvokableSingleArg> poolElement)
        {
            PoolElement = poolElement;

            Publisher = publisher;
            
            Active = true;
        }
        
        public void Terminate()
        {
            PoolElement = null;
            
            Publisher = null;
            
            Active = false;
        }

        #endregion
    }
}
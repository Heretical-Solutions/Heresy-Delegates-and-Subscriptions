using System;

using HereticalSolutions.Collections;
using HereticalSolutions.Pools;

namespace HereticalSolutions.Delegates.Pinging
{
	public class NonAllocPinger
		: IPublisherNoArgs,
		  INonAllocSubscribableNoArgs
	{
		#region Subscriptions
		
		private readonly INonAllocDecoratedPool<IInvokableNoArgs> subscriptionsPool;

		private readonly IIndexable<IPoolElement<IInvokableNoArgs>> subscriptionsAsIndexable;

		private readonly IFixedSizeCollection<IPoolElement<IInvokableNoArgs>> subscriptionsWithCapacity;

		#endregion
		
		#region Buffer

		private IInvokableNoArgs[] currentSubscriptionsBuffer;

		private int currentSubscriptionsBufferCount = -1;

		#endregion
		
		private bool pingInProgress = false;

		public NonAllocPinger(
			INonAllocDecoratedPool<IInvokableNoArgs> subscriptionsPool,
			INonAllocPool<IInvokableNoArgs> subscriptionsContents)
		{
			this.subscriptionsPool = subscriptionsPool;

			subscriptionsAsIndexable = (IIndexable<IPoolElement<IInvokableNoArgs>>)subscriptionsContents;

			subscriptionsWithCapacity =
				(IFixedSizeCollection<IPoolElement<IInvokableNoArgs>>)subscriptionsContents;

			currentSubscriptionsBuffer = new IInvokableNoArgs[subscriptionsWithCapacity.Capacity];
		}

		#region INonAllocSubscribableNoArgs
		
		public void Subscribe(ISubscriptionHandler<INonAllocSubscribableNoArgs, IInvokableNoArgs> subscription)
		{
			#region Validate
			
			if (subscription.Active)
				throw new Exception("[NonAllocPinger] ATTEMPT TO ACTIVATE A SUBSCRIPTION THAT IS ALREADY ACTIVE");
			
			if (subscription.Publisher != null)
				throw new Exception("[NonAllocPinger] SUBSCRIPTION ALREADY HAS A PUBLISHER");
			
			if (subscription.PoolElement != null)
				throw new Exception("[NonAllocPinger] SUBSCRIPTION ALREADY HAS A POOL ELEMENT");
			
			if (subscription.Delegate == null)
				throw new Exception("[NonAllocPinger] INVALID DELEGATE");
			
			#endregion
			
			var subscriptionElement = subscriptionsPool.Pop(null);

			subscriptionElement.Value = subscription.Delegate;

			subscription.Activate(this, subscriptionElement);
		}

		public void Unsubscribe(ISubscriptionHandler<INonAllocSubscribableNoArgs, IInvokableNoArgs> subscription)
		{
			#region Validate
			
			if (!subscription.Active)
				throw new Exception("[NonAllocPinger] ATTEMPT TO TERMINATE A SUBSCRIPTION THAT IS ALREADY ACTIVE");
			
			if (subscription.Publisher != this)
				throw new Exception("[NonAllocPinger] INVALID PUBLISHER");
			
			if (subscription.PoolElement == null)
				throw new Exception("[NonAllocPinger] INVALID POOL ELEMENT");
			
			#endregion

			var poolElement = subscription.PoolElement;
			
			TryRemoveFromBuffer(poolElement);
			
			poolElement.Value = null;

			subscriptionsPool.Push(poolElement);
			
			subscription.Terminate();
		}

		public void Unsubscribe(IPoolElement<IInvokableNoArgs> subscription)
		{
			TryRemoveFromBuffer(subscription);
			
			subscription.Value = null;

			subscriptionsPool.Push(subscription);
		}

		private void TryRemoveFromBuffer(IPoolElement<IInvokableNoArgs> subscriptionElement)
		{
			if (!pingInProgress)
				return;
				
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				if (currentSubscriptionsBuffer[i] == subscriptionElement.Value)
				{
					currentSubscriptionsBuffer[i] = null;

					return;
				}
		}
		
		#endregion

		#region IPublisherNoArgs
		
		public void Publish()
		{
			ValidateBufferSize();

			currentSubscriptionsBufferCount = subscriptionsAsIndexable.Count;

			CopySubscriptionsToBuffer();

			InvokeSubscriptions();

			EmptyBuffer();
		}

		private void ValidateBufferSize()
		{
			if (currentSubscriptionsBuffer.Length < subscriptionsWithCapacity.Capacity)
				currentSubscriptionsBuffer = new IInvokableNoArgs[subscriptionsWithCapacity.Capacity];
		}

		private void CopySubscriptionsToBuffer()
		{
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				currentSubscriptionsBuffer[i] = subscriptionsAsIndexable[i].Value;
		}

		private void InvokeSubscriptions()
		{
			pingInProgress = true;

			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
			{
				if (currentSubscriptionsBuffer[i] != null)
					currentSubscriptionsBuffer[i].Invoke();
			}

			pingInProgress = false;
		}

		private void EmptyBuffer()
		{
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				currentSubscriptionsBuffer[i] = null;
		}
		
		#endregion
	}
}
using HereticalSolutions.Collections;
using HereticalSolutions.Pools;

namespace HereticalSolutions.Delegates.Broadcasting
{
	public class BroadcasterNonAlloc<TValue>
		: IPublisherSingleArgument,
		  IPublisherSingleArgumentGeneric<TValue>,
		  ISubscribableNonAlloc<IBroadcastHandlerGeneric<TValue>>
	{
		#region Subscriptions
		
		private INonAllocPool<IBroadcastHandlerGeneric<TValue>> subscriptionsPool;

		private IIndexable<IPoolElement<IBroadcastHandlerGeneric<TValue>>> subscriptionsAsIndexable;
		
		private IFixedSizeCollection<IPoolElement<IBroadcastHandlerGeneric<TValue>>> subscriptionsWithCapacity;

		#endregion
		
		#region Buffer
		
		private IBroadcastHandlerGeneric<TValue>[] currentSubscriptionsBuffer;

		private int currentSubscriptionsBufferCount = -1;

		#endregion
		
		private bool broadcastInProgress = false;

		public BroadcasterNonAlloc(
			INonAllocPool<IBroadcastHandlerGeneric<TValue>> subscriptionsPool,
			INonAllocPool<IBroadcastHandlerGeneric<TValue>> subscriptionsContents)
		{
			this.subscriptionsPool = subscriptionsPool;

			subscriptionsAsIndexable = (IIndexable<IPoolElement<IBroadcastHandlerGeneric<TValue>>>)subscriptionsContents;

			subscriptionsWithCapacity =
				(IFixedSizeCollection<IPoolElement<IBroadcastHandlerGeneric<TValue>>>)subscriptionsContents;

			currentSubscriptionsBuffer = new IBroadcastHandlerGeneric<TValue>[subscriptionsWithCapacity.Capacity];
		}

		#region ISubscribableNonAlloc
		
		public IPoolElement<IBroadcastHandlerGeneric<TValue>> Subscribe(IBroadcastHandlerGeneric<TValue> handlerGeneric)
		{
			var subscriptionElement = subscriptionsPool.Pop();

			subscriptionElement.Value = handlerGeneric;

			return subscriptionElement;
		}

		public void Unsubscribe(IPoolElement<IBroadcastHandlerGeneric<TValue>> subscription)
		{
			TryUnsubscribeFromBuffer(subscription);

			subscription.Value = null;

			subscriptionsPool.Push(subscription);
		}
		
		private void TryUnsubscribeFromBuffer(IPoolElement<IBroadcastHandlerGeneric<TValue>> subscriptionElement)
		{
			if (!broadcastInProgress)
				return;
				
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				if (currentSubscriptionsBuffer[i] == subscriptionElement.Value)
				{
					currentSubscriptionsBuffer[i] = null;

					return;
				}
		}
		
		#endregion

		#region IBroadcastable, IBroadcastableGeneric
		
		public void Publish(TValue value)
		{
			ValidateBufferSize();

			currentSubscriptionsBufferCount = subscriptionsAsIndexable.Count;

			CopySubscriptionsToBuffer();

			HandleSubscriptions(value);

			EmptyBuffer();
		}

		public void Publish(object value)
		{
			Publish((TValue)value);
		}

		public void Publish<TInherited>(TInherited value) where TInherited : TValue
		{
			Publish((TValue)value);
		}

		private void ValidateBufferSize()
		{
			if (currentSubscriptionsBuffer.Length < subscriptionsWithCapacity.Capacity)
				currentSubscriptionsBuffer = new IBroadcastHandlerGeneric<TValue>[subscriptionsWithCapacity.Capacity];
		}

		private void CopySubscriptionsToBuffer()
		{
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				currentSubscriptionsBuffer[i] = subscriptionsAsIndexable[i].Value;
		}

		private void HandleSubscriptions(TValue value)
		{
			broadcastInProgress = true;

			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
			{
				if (currentSubscriptionsBuffer[i] != null)
					currentSubscriptionsBuffer[i].Handle(value);
			}

			broadcastInProgress = false;
		}

		private void EmptyBuffer()
		{
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				currentSubscriptionsBuffer[i] = null;
		}
		
		#endregion
	}
}
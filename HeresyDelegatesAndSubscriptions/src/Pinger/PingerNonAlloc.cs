using HereticalSolutions.Collections;
using HereticalSolutions.Pools;

namespace HereticalSolutions.Delegates.Pinging
{
	public class PingerNonAlloc
		: IPublisherNoArgs,
		  INonAllocSubscribableNoArgs
	{
		#region Subscriptions
		
		private INonAllocPool<IPingHandler> subscriptionsPool;

		private IIndexable<IPoolElement<IPingHandler>> subscriptionsAsIndexable;

		private IFixedSizeCollection<IPoolElement<IPingHandler>> subscriptionsWithCapacity;

		#endregion
		
		#region Buffer

		private IPingHandler[] currentSubscriptionsBuffer;

		private int currentSubscriptionsBufferCount = -1;

		#endregion
		
		private bool pingInProgress = false;

		public Pinger(
			INonAllocPool<IPingHandler> subscriptionsPool,
			INonAllocPool<IPingHandler> subscriptionsContents)
		{
			this.subscriptionsPool = subscriptionsPool;

			subscriptionsAsIndexable = (IIndexable<IPoolElement<IPingHandler>>)subscriptionsContents;

			subscriptionsWithCapacity =
				(IFixedSizeCollection<IPoolElement<IPingHandler>>)subscriptionsContents;

			currentSubscriptionsBuffer = new IPingHandler[subscriptionsWithCapacity.Capacity];
		}

		#region ISubscribableNonAlloc
		
		public IPoolElement<IPingHandler> Subscribe(IPingHandler handler)
		{
			var subscriptionElement = subscriptionsPool.Pop();

			subscriptionElement.Value = handler;

			return subscriptionElement;
		}

		public void Unsubscribe(IPoolElement<IPingHandler> subscription)
		{
			TryUnsubscribeFromBuffer(subscription);
			
			subscription.Value = null;

			subscriptionsPool.Push(subscription);
		}

		private void TryUnsubscribeFromBuffer(IPoolElement<IPingHandler> subscriptionElement)
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

		#region IPingable
		
		public void Publish()
		{
			ValidateBufferSize();

			currentSubscriptionsBufferCount = subscriptionsAsIndexable.Count;

			CopySubscriptionsToBuffer();

			HandleSubscriptions();

			EmptyBuffer();
		}

		private void ValidateBufferSize()
		{
			if (currentSubscriptionsBuffer.Length < subscriptionsWithCapacity.Capacity)
				currentSubscriptionsBuffer = new IPingHandler[subscriptionsWithCapacity.Capacity];
		}

		private void CopySubscriptionsToBuffer()
		{
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				currentSubscriptionsBuffer[i] = subscriptionsAsIndexable[i].Value;
		}

		private void HandleSubscriptions()
		{
			pingInProgress = true;

			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
			{
				if (currentSubscriptionsBuffer[i] != null)
					currentSubscriptionsBuffer[i].Handle();
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
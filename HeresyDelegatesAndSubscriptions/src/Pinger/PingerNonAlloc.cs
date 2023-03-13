using HereticalSolutions.Collections;
using HereticalSolutions.Pools;

namespace HereticalSolutions.Delegates.Pinging
{
	public class PingerNonAlloc
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

		public PingerNonAlloc(
			INonAllocDecoratedPool<IInvokableNoArgs> subscriptionsPool,
			INonAllocDecoratedPool<IInvokableNoArgs> subscriptionsContents)
		{
			this.subscriptionsPool = subscriptionsPool;

			subscriptionsAsIndexable = (IIndexable<IPoolElement<IInvokableNoArgs>>)subscriptionsContents;

			subscriptionsWithCapacity =
				(IFixedSizeCollection<IPoolElement<IInvokableNoArgs>>)subscriptionsContents;

			currentSubscriptionsBuffer = new IInvokableNoArgs[subscriptionsWithCapacity.Capacity];
		}

		#region ISubscribableNonAlloc
		
		public IPoolElement<IInvokableNoArgs> Subscribe(IInvokableNoArgs handler)
		{
			var subscriptionElement = subscriptionsPool.Pop();

			subscriptionElement.Value = handler;

			return subscriptionElement;
		}

		public void Unsubscribe(IPoolElement<IInvokableNoArgs> subscription)
		{
			TryUnsubscribeFromBuffer(subscription);
			
			subscription.Value = null;

			subscriptionsPool.Push(subscription);
		}

		private void TryUnsubscribeFromBuffer(IPoolElement<IInvokableNoArgs> subscriptionElement)
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
				currentSubscriptionsBuffer = new IInvokableNoArgs[subscriptionsWithCapacity.Capacity];
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
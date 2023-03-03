using System;

namespace HereticalSolutions.Delegates.Broadcasting
{
	public class BroadcasterGeneric<TArgument>
		: IPublisherSingleArgumentGeneric<TArgument>,
		  ISubscribableSingleArgumentGeneric<TArgument>
	{
		#region Subscriptions

		private Action<TArgument> multicastDelegate;
		
		#endregion

		#region IPublisherSingleArgumentGeneric

		public void Publish(TArgument value)
		{
			multicastDelegate?.Invoke(value);
		}

		#endregion

		#region ISubscribableSingleArgumentGeneric
		
		public void Subscribe(Action<TArgument> @delegate)
		{
			multicastDelegate += @delegate;
		}

		public void Unsubscribe(Action<TArgument> @delegate)
		{
			multicastDelegate -= @delegate;
		}
		
		#endregion
	}
}
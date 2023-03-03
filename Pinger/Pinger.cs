using System;

namespace HereticalSolutions.Delegates.Pinging
{
	public class Pinger
		: IPublisher,
		  ISubscribable
	{
		#region Subscriptions
		
		private Action multicastDelegate;

		#endregion

		#region IPublisher
		
		public void Publish()
		{
			multicastDelegate?.Invoke();
		}

		#endregion
		
		#region ISubscribable
		
		public void Subscribe(Action @delegate)
		{
			multicastDelegate += @delegate;
		}
        
		public void Unsubscribe(Action @delegate)
		{
			multicastDelegate -= @delegate;
		}
        
		#endregion
	}
}
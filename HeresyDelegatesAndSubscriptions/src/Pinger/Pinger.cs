using System;

namespace HereticalSolutions.Delegates.Pinging
{
	public class Pinger
		: IPublisherNoArgs,
		  ISubscribableNoArgs
	{
		private Action multicastDelegate;

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
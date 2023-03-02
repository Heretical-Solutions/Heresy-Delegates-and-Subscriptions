using System;


delegate void TestDelegate(object arg);

namespace HereticalSolutions.Delegates.Broadcasting
{
	public class Broadcaster<TArgument>
		: IPublisherSingleArgument,
		  IPublisherSingleArgumentGeneric<TArgument>,
		  ISubscribableSingleArgument,
		  ISubscribableSingleArgumentGeneric<TArgument>
	{
		#region Subscriptions

		private Action<object> multicastDelegate;
		
		#endregion

		#region IPublisherSingleArgument

		public void Publish(object value)
		{
			multicastDelegate?.Invoke((TArgument)value);
		}

		public void Publish<TValue>(TValue value) where TValue : TArgument
		{
			multicastDelegate?.Invoke(value);
		}

		#endregion

		#region IPublisherSingleArgumentGeneric

		public void Publish(TArgument value)
		{
			multicastDelegate?.Invoke(value);
		}

		#endregion

		#region ISubscribableSingleArgument
		
		public void Subscribe<TArgument1>(object @delegate)
		{
			throw new NotImplementedException();
		}

		private void Test(int arg)
		{
		}

		public void Subscribe<TValue>(Action<TValue> @delegate) where TValue : TArgument
		{
			TestDelegate temp = new TestDelegate(Test);
			
			multicastDelegate += @delegate;
		}

		public void Unsubscribe<TArgument1>(object @delegate)
		{
			throw new NotImplementedException();
		}

		public void Unsubscribe<TArgument1>(Action<TArgument1> @delegate)
		{
			throw new NotImplementedException();
		}
		
		#endregion

		#region ISubscribableSingleArgumentGeneric
		
		public void Subscribe(object @delegate)
		{
			throw new NotImplementedException();
		}

		public void Subscribe(Action<TArgument> @delegate)
		{
			throw new NotImplementedException();
		}

		public void Unsubscribe(object @delegate)
		{
			throw new NotImplementedException();
		}

		public void Unsubscribe(Action<TArgument> @delegate)
		{
			throw new NotImplementedException();
		}
		
		#endregion
	}
}
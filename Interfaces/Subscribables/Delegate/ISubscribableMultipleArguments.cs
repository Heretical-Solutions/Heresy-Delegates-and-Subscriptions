using System;

namespace HereticalSolutions.Delegates
{
    public interface ISubscribableMultipleArguments
    {
        void Subscribe(Action<object[]> @delegate);

        void Unsubscribe(Action<object[]> @delegate);
    }
}
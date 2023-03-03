using System;

namespace HereticalSolutions.Delegates
{
    public interface ISubscribableSingleArgumentGeneric<TValue>
    {
        void Subscribe(Action<TValue> @delegate);

        void Unsubscribe(Action<TValue> @delegate);
    }
}
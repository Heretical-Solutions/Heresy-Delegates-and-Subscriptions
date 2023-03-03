using System;

namespace HereticalSolutions.Delegates
{
    public interface ISubscribableSingleArgumentGeneric<TArgument>
    {
        void Subscribe(Action<TArgument> @delegate);

        void Unsubscribe(Action<TArgument> @delegate);
    }
}
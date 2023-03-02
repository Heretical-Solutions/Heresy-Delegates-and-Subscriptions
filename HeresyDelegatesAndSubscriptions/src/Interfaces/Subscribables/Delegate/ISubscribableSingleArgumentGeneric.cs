using System;

namespace HereticalSolutions.Delegates
{
    public interface ISubscribableSingleArgumentGeneric<TArgument>
    {
        void Subscribe(object @delegate);
        
        void Subscribe(Action<TArgument> @delegate);

        void Unsubscribe(object @delegate);
        
        void Unsubscribe(Action<TArgument> @delegate);
    }
}
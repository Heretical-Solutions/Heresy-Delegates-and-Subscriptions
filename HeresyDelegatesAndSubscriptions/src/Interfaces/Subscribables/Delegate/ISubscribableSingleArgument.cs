using System;

namespace HereticalSolutions.Delegates
{
    public interface ISubscribableSingleArgument
    {
        void Subscribe(object @delegate);
        
        void Subscribe<TArgument>(Action<TArgument> @delegate);

        void Unsubscribe(object @delegate);
        
        void Unsubscribe<TArgument>(Action<TArgument> @delegate);
    }
}
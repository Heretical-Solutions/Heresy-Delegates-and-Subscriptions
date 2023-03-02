using System;

namespace HereticalSolutions.Delegates
{
    public interface ISubscribableSingleArgument
    {
        void Subscribe<TArgument>(object @delegate);
        
        void Subscribe<TArgument>(Action<TArgument> @delegate);

        void Unsubscribe<TArgument>(object @delegate);
        
        void Unsubscribe<TArgument>(Action<TArgument> @delegate);
    }
}
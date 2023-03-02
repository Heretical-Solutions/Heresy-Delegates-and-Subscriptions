using System;

namespace HereticalSolutions.Delegates
{
    public interface ISubscribableMultipleArguments
    {
        void Subscribe(object @delegate);
        
        void Subscribe(Action<object[]> @delegate);

        void Unsubscribe(object @delegate);
        
        void Unsubscribe(Action<object[]> @delegate);
    }
}
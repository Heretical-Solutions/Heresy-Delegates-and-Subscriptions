using System;

namespace HereticalSolutions.Delegates
{
    public interface ISubscribableNoArguments
    {
        void Subscribe(Action @delegate);
        
        void Unsubscribe(Action @delegate);
    }
}
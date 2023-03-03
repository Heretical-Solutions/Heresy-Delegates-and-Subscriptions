using System;

namespace HereticalSolutions.Delegates
{
    public interface ISubscribable
    {
        void Subscribe(Action @delegate);
        
        void Unsubscribe(Action @delegate);
    }
}
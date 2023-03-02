using System;

namespace HereticalSolutions.Delegates
{
    public interface ISubscribable
    {
        void Subscribe(Action @delegate);
        
        void Subscribe(object @delegate);

        void Unsubscribe(Action @delegate);
        
        void Unsubscribe(object @delegate);
    }
}
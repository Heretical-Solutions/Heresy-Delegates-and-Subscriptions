using System;

namespace HereticalSolutions.Delegates.Wrappers
{
    public class DelegateWrapperSingleArg : IInvokableSingleArg
    {
        private readonly Action<object> @delegate;

        public DelegateWrapperSingleArg(Action<object> @delegate)
        {
            this.@delegate = @delegate;
        }

        public void Invoke<TValue>(TValue argument)
        {
            @delegate?.Invoke(argument);
        }
    }
}
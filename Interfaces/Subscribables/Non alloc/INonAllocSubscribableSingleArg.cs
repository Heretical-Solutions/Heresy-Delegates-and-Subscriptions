namespace HereticalSolutions.Delegates
{
    public interface INonAllocSubscribableSingleArg
    {
        void Subscribe<TValue>(ISubscriptionHandler<INonAllocSubscribableSingleArg, IInvokableSingleArg> subscription);

        void Unsubscribe<TValue>(ISubscriptionHandler<INonAllocSubscribableSingleArg, IInvokableSingleArg> subscription);
    }
}
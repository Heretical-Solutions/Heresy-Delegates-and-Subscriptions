namespace HereticalSolutions.Delegates
{
    public interface ISubscribableNonAllocNoArguments<TValue>
    {
        IPoolElement<TValue> Subscribe(TValue handler);

        void Unsubscribe(IPoolElement<TValue> subscription);
    }
}
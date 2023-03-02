namespace HereticalSolutions.Delegates
{
    public interface ISubscribableNonAlloc<TValue>
    {
        IPoolElement<TValue> Subscribe(TValue handler);

        void Unsubscribe(IPoolElement<TValue> subscription);
    }
}
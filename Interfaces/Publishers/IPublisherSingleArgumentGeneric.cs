namespace HereticalSolutions.Delegates
{
    public interface IPublisherSingleArgumentGeneric<TValue>
    {
        void Publish(TValue value);
    }
}
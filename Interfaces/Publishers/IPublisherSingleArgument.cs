namespace HereticalSolutions.Delegates
{
    public interface IPublisherSingleArgument
    {
        void Publish<TValue>(TValue value);
    }
}
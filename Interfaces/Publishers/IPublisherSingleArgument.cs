namespace HereticalSolutions.Delegates
{
    public interface IPublisherSingleArgument
    {
        void Publish(object value);

        void Publish<TValue>(TValue value);
    }
}
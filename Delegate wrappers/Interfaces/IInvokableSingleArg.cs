namespace HereticalSolutions.Delegates
{
    public interface IInvokableSingleArg
    {
        void Invoke<TValue>(TValue arg);
    }
}
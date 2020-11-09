namespace TestApp.Cli.Commands.MessagePipeline.ChainStyleImpl
{
    public interface IChainStep<T>
    {
        bool Execute(T msg);
        IChainStep<T> NextStep(IChainStep<T> next);
    }
}
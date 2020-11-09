namespace TestApp.Cli.Commands.MessagePipeline.ChainStyleImpl
{
    abstract public class ChainStep<T> : IChainStep<T>
    {
        IChainStep<T> nextStep;

        public bool Execute(T msg)
        {
            if(Handle(msg))
            {
                if (nextStep != null)
                    return nextStep.Execute(msg);
            }

            return false;
        }

        abstract protected bool Handle(T msg);

        public IChainStep<T> NextStep(IChainStep<T> next)
        {
            nextStep = next;
            return nextStep;
        }
    }
}
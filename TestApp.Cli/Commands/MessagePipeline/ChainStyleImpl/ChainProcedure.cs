using System;

namespace TestApp.Cli.Commands.MessagePipeline.ChainStyleImpl
{
    public class ChainProcedure<T>
    {
        IChainStep<T> firstStep;
        IChainStep<T> lastStep;

        public ChainProcedure<T> AddStep(IChainStep<T> next)
        {
            if (firstStep == null)
                firstStep = next;
            lastStep?.NextStep(next);
            lastStep = next;
            return this;
        }

        public ChainProcedure<T> AddStep(Func<T, bool> fn)
        {
            return this.AddStep(new GeneralChainStep<T>(fn));
        }

        public void Execute(T msg)
        {
            firstStep?.Execute(msg);
        }
    }
}
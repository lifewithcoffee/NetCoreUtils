using System;

namespace TestApp.Cli.Commands.MessagePipeline.ChainStyleImpl
{
    public class GeneralChainStep<T> : ChainStep<T>
    {

        Func<T, bool> _fn = T => { return false; };

        public GeneralChainStep(Func<T,bool> fn)
        {
            _fn = fn;
        }

        protected override bool Handle(T msg)
        {
            return _fn(msg);
        }
    }
}
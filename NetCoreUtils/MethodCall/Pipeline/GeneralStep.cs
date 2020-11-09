using System;

namespace NetCoreUtils.MethodCall.Pipeline
{
    public class GeneralStep<T> : IStep<T>
    {
        Func<T, bool> _fn = T => { return false; };

        public GeneralStep(Func<T, bool> fn)
        {
            _fn = fn;
        }

        public bool Execute(T msg)
        {
            return _fn(msg);
        }
    }
}
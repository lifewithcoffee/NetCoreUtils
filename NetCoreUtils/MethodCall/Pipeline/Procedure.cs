using System;
using System.Collections.Generic;

namespace NetCoreUtils.MethodCall.Pipeline
{
    public class Procedure<T>
    {
        List<IStep<T>> steps = new List<IStep<T>>();

        public Procedure<T> AddStep(IStep<T> step)
        {
            steps.Add(step);
            return this;
        }

        public Procedure<T> AddStep(Func<T, bool> fn)
        {
            steps.Add(new GeneralStep<T>(fn));
            return this;
        }

        public void Execute(T msg)
        {
            foreach(var step in steps)
            {
                if (!step.Execute(msg))
                    break;
            }
        }
    }
}
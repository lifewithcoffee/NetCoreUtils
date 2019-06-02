using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreUtils.MethodCall
{
    public static class PipeForwardExt
    {
        public static void Forward<T>(this T v, Action<T> f) => f(v);
        public static TOut Forward<TIn, TOut>(this TIn v, Func<TIn, TOut> f) => f(v);
    }
}

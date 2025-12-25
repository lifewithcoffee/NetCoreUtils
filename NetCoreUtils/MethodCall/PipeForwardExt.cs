using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetCoreUtils.MethodCall;

/**
 * Usage:
 *  public static void Demo()
 *  {
 *      var result = 5
 *          >> (x => x * 2)
 *          >> (x => x + 3);
 *      Console.WriteLine(result); // operator overloading can't return void, so can't further do: >> (onsole.WriteLine);
 *
 *      5.Forward(x => x * 2)
 *       .Forward(x => x + 3)
 *       .Forward(Console.WriteLine);
 *  }
 */
public static class PipeForwardExt
{
    public static void Forward<T>(this T v, Action<T> f) => f(v);
    public static TOut Forward<TIn, TOut>(this TIn v, Func<TIn, TOut> f) => f(v);

    /**
     * Only for experimenting C#'s new "extension block" feature,
     * don't enable this, it's a bad practice to use operator overloading like this.
     */
    //extension<TIn, TOut>(TIn)
    //{
    //    public static TOut operator >> (TIn source, Func<TIn, TOut> f) => f(source);
    //}
}

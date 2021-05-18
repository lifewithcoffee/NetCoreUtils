using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NetCoreUtils.Lang.Expression
{
    static public class ExpressionUtil
    {
        static public TProperty GetValue<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> expression)
        {
            var value = expression.Compile()(source);
            return value;
        }
    }
}

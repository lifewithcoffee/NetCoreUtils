using NetCoreUtils.TestCli.ReflectUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreUtils.TestCli.Commands
{
    public class ReflectCommand
    {
        public void CallImpls2()
        {
            Console.WriteLine("Foo2() called");
            var list = new ReflectUtil().GetAllImplementationInstances2(typeof(ISomeInterface));
            foreach (var obj in list)
            {
                ((ISomeInterface)obj).DoSomething();
            }
        }

        public void CallImpls1()
        {
            Console.WriteLine("Foo1() called");
            new ReflectUtil().GetAllImplementationInstances1<ISomeInterface>().ForEach(x => x.DoSomething());
        }
    }
}

using NetCoreUtils.Reflection;
using NetCoreUtils.TestCli.ReflectUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreUtils.TestCli.Commands
{
    public interface ISomeInterface
    {
        void DoSomething();
    }

    public class Impl1 : ISomeInterface
    {
        public void DoSomething()
        {
            Console.WriteLine("Do1.DoSomething() called");
        }
    }

    public class Impl2 : ISomeInterface
    {
        public void DoSomething()
        {
            Console.WriteLine("Do2.DoSomething() called");
        }
    }

    public class ReflectCommand
    {
        public void Foo2()
        {
            Console.WriteLine("Foo2() called");
            var list = new ReflectUtil().GetAllImplementationInstances2(typeof(Program).Assembly, typeof(ISomeInterface));
            foreach (var obj in list)
            {
                ((ISomeInterface)obj).DoSomething();
            }
        }

        public void Foo1()
        {
            Console.WriteLine("Foo1() called");
            new ReflectUtil().GetAllImplementationInstances1<ISomeInterface>(typeof(Program).Assembly).ForEach(x => x.DoSomething());
        }
    }
}

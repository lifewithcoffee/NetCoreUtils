using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreUtils.TestCli.ReflectUtils
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

    /// <summary>
    /// Methods in this class can be used to implemnt an DI auto-injector
    /// </summary>
    public class ReflectUtil
    {

        public IEnumerable<object> GetAllImplementationInstances2(Type interfaceType)
        {
            var interfaceImpls = typeof(Program)
                .Assembly
                .ExportedTypes
                .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)      // make sure the type is an initializable class that implements the interface
                .Select(Activator.CreateInstance);

            return interfaceImpls;
        }

        public List<T> GetAllImplementationInstances1<T>()
        {
            var interfaceImpls = typeof(Program)
                .Assembly
                .ExportedTypes
                .Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)      // make sure the type is an initializable class that implements the interface
                .Select(Activator.CreateInstance).Cast<T>().ToList();

            return interfaceImpls;
        }
    }
}

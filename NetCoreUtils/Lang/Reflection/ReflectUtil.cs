using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetCoreUtils.Lang.Reflection
{
    /// <summary>
    /// Methods in this class can be used to implemnt an DI auto-injector
    /// </summary>
    public class ReflectUtil
    {

        public IEnumerable<object> GetAllImplementationInstances2(Assembly assembly, Type interfaceType)
        {
            var interfaceImpls = assembly
                .ExportedTypes
                .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)      // make sure the type is an initializable class that implements the interface
                .Select(Activator.CreateInstance);

            return interfaceImpls;
        }

        public List<T> GetAllImplementationInstances1<T>(Assembly assembly)
        {
            var interfaceImpls = assembly
                .ExportedTypes
                .Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)      // make sure the type is an initializable class that implements the interface
                .Select(Activator.CreateInstance).Cast<T>().ToList();

            return interfaceImpls;
        }
    }
}

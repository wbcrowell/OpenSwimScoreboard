using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace OpenSwimScoreboard.Utilities
{
    public static class TypeUtilities
    {
        public static IEnumerable<Type> GetImplementationsOfInterface<T>()
        {
            Type interfaceType = typeof(T);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            IEnumerable<Type> subclasses = types.Where(t => t.GetInterfaces().Any(i => i.Name == interfaceType.Name));

            return subclasses;
        }
    }
}

using System;
using System.Linq;

namespace Com.MyCompany.MyGame.Common
{
    public static class InheritHelper
    {
        public static Type[] GetAllImplementations<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(T).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
                .ToArray();
        }
    }
}
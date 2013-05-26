using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ClassLibraryHelper.C_
{
    public static class Singleton<T> where T : class
    {
        private static object _lock=new object();

        static Singleton()
        {
        }

        private static T _instance;
        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        var container = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
                        if (container == null || container.IsAssembly)
                        {
                            throw new Exception("can't find container in class" + typeof(T).Name);
                        }
                       _instance = (T)container.Invoke(null);
                    }
                }
                return _instance;
            }
        }
    }
}

using System;
using System.Diagnostics;

namespace NotDI
{
    public class Resolver
    {
        private Resolver _mainInstance = null;

        private Resolver()
        {

        }

        public static Session NewSession()
        {
            return Session.ResolveInstance();
        }

        public static T1 ResolveIt<T1>()
        {
            using (var session = Session.ResolveInstance())
            {
                return session.Resolve<T1>();
            }
        }

        public static void ResolveIt<T1>(out T1 t1)
        {
            using (var session = Session.ResolveInstance())
            {
                t1 = session.Resolve<T1>();
            }
        }

        public static (T1, T2) ResolveIt<T1, T2>() 
        {
            using (var session = Session.ResolveInstance())
            {
                var res = session.Resolve(typeof(T1), typeof(T2));
                return ((T1)res[0], (T2)res[1]);
            }
        }

        public static void ResolveIt<T1, T2>(out T1 t1, out T2 t2)
        {
            using (var session = Session.ResolveInstance())
            {
                var res = session.Resolve(typeof(T1), typeof(T2));
                t1 = (T1)res[0];
                t2 = (T2)res[1];
            }
        }
    }
}

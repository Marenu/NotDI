using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;

namespace NotDI
{
    public class Session : IDisposable
    {
        private static Mapper _mapper = null;

        private static Dictionary<Type, object> _singletonLookup = new Dictionary<Type, object>();

        private static Dictionary<int, Session> _scopeInstances = new Dictionary<int, Session>();
        private Dictionary<Type, object> _scopedLookup;
        private int _scopedDepth;

        private Session()
        {

        }

        internal static Session ResolveInstance()
        {
            lock (_scopeInstances)
            {
                if (_scopeInstances.TryGetValue(Thread.CurrentThread.ManagedThreadId, out Session existingSession))
                {
                    existingSession._scopedDepth++;
                    return existingSession;
                }
                var newSession = new Session();
                _scopeInstances.Add(Thread.CurrentThread.ManagedThreadId, newSession);
                return newSession;
            }
        }

        public T Resolve<T>()
        {
            return (T)ResolveIt(typeof(T));
        }

        public object[] Resolve(params Type[] types)
        {
            var objs = new object[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                objs[i] = ResolveIt(types[i]);
            }
            return objs;
        }

        private object ResolveIt(Type type)
        {
            if (_mapper == null)
                _mapper = SearchMapper();
            _mapper.Resolve(type, out Type resolved, out MappingType mappingType);
            switch (mappingType)
            {
                case MappingType.Singleton:
                    lock (_singletonLookup)
                    {
                        if (_singletonLookup.TryGetValue(type, out object singletonInstance))
                            return singletonInstance;
                        var newSingletonInstance = Activator.CreateInstance(resolved);
                        _singletonLookup.Add(type, newSingletonInstance);
                        return newSingletonInstance;
                    }
                case MappingType.SessionScoped:
                    if (_scopedLookup == null)
                        _scopedLookup = new Dictionary<Type, object>();
                    if (_scopedLookup.TryGetValue(type, out object scopedInstance))
                        return scopedInstance;
                    var newScopedInstance = Activator.CreateInstance(resolved);
                    _scopedLookup.Add(type, newScopedInstance);
                    return newScopedInstance;
                case MappingType.Transient:
                    return Activator.CreateInstance(resolved);
            }
            throw new Exception("Something goes wrong!");
        }

        private Mapper SearchMapper()
        {
            var cType = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(p => typeof(ICreateMappings).IsAssignableFrom(p))
                .FirstOrDefault();

            if (cType == null)
                throw new Exception("Mappings not found");

            var mapperClass = (ICreateMappings)Activator.CreateInstance(cType);

            return mapperClass.CreateMappings(new Mapper());
        }

        public void Dispose()
        {
            if (_scopedDepth > 0)
            {
                _scopedDepth--;
            }
            else
            {
                lock (_scopeInstances)
                {
                    _scopeInstances.Remove(Thread.CurrentThread.ManagedThreadId);
                }
            }
        }
    }
}

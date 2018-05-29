using System;
using System.Collections.Generic;
using System.Text;

namespace NotDI
{
    public class Mapper
    {
        internal static Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();
        internal static Dictionary<Type, MappingType> _mappingTypes = new Dictionary<Type, MappingType>();

        internal Mapper()
        {

        }

        public void AddSingleton<TSource, TTarget>()
        {
            _mappings.Add(typeof(TSource), typeof(TTarget));
            _mappingTypes.Add(typeof(TSource), MappingType.Singleton);
        }

        public void AddSessionScoped<TSource, TTarget>()
        {
            _mappings.Add(typeof(TSource), typeof(TTarget));
            _mappingTypes.Add(typeof(TSource), MappingType.SessionScoped);
        }

        public void AddTransient<TSource, TTarget>()
        {
            _mappings.Add(typeof(TSource), typeof(TTarget));
            _mappingTypes.Add(typeof(TSource), MappingType.Transient);
        }

        internal void Resolve(Type type, out Type resolved, out MappingType mappingType)
        {
            if (!_mappings.TryGetValue(type, out resolved))
                throw new Exception("Mapping not found");
            mappingType = _mappingTypes[type];
        }
    }

    internal enum MappingType
    {
        Singleton,
        SessionScoped,
        Transient,
    }
}

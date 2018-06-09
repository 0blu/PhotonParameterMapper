using System;
using System.Collections.Generic;
using System.Data;

namespace PhotonParameterMapper.Core
{
    /// <summary>
    /// A simple biserializerRegistry for <see cref="ICustomTypeBiserializer{T}"/>
    /// </summary>
    public class SimpleCustomTypeBiserializerRegistry : ICustomTypeBiserializerRegistry
    {
        private readonly Dictionary<Type, object> _storedInstances = new Dictionary<Type, object>();

        /// <summary>
        /// Adds a new biserializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TBiserializer"></typeparam>
        public void RegisterCustomTypeBiserializer<T, TBiserializer>() where TBiserializer : ICustomTypeBiserializer<T>, new()
        {
            RegisterCustomTypeBiserializer(new TBiserializer());
        }

        /// <summary>
        /// Adds a new biserializer
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterCustomTypeBiserializer<T>(ICustomTypeBiserializer<T> instance)
        {
            _storedInstances.Add(typeof(T), instance);
        }

        /// <inheritdoc />
        public ICustomTypeBiserializer<T> GetCustomTypeBiserializer<T>()
        {
            if (!_storedInstances.TryGetValue(typeof(T), out object instance))
                throw new NoNullAllowedException($"No biserializer for '{typeof(T)}' was registerd");

            return (ICustomTypeBiserializer<T>)instance;
        }
    }
}

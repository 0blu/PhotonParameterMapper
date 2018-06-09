using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PhotonParameterMapper.Example.SpeedComparison
{
    public class ReflectionMapper
    {
        private readonly Dictionary<Type, List<ReflectionMappersCustomProperty>> _contractCustomProperty = new Dictionary<Type, List<ReflectionMappersCustomProperty>>();

        private static void GetInheritanceHierarchy(Type type, List<Type> parents)
        {
            if (type.BaseType == null)
                return;

            parents.Add(type);
            GetInheritanceHierarchy(type.BaseType, parents);
        }

        public void RegisterContract<T>()
        {
            RegisterContract(typeof(T));
        }

        public void RegisterContract(Type contractType)
        {
            List<Type> fieldTypes = new List<Type>();
            GetInheritanceHierarchy(contractType, fieldTypes);
            fieldTypes.Reverse();

            List<ReflectionMappersCustomProperty> customProperties = new List<ReflectionMappersCustomProperty>();
            foreach (Type fieldType in fieldTypes)
            {
                FieldInfo[] fields = fieldType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                IOrderedEnumerable<FieldInfo> source = from x in fields orderby x.MetadataToken select x;
                foreach (FieldInfo current3 in source.ToList())
                {
                    ReflectionMappersCustomProperty item = new ReflectionMappersCustomProperty(current3);
                    customProperties.Add(item);
                }
            }
            _contractCustomProperty.Add(contractType, customProperties);
        }

        public Dictionary<byte, object> ToDictionary(Type contractType, object contract)
        {
            Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
            _contractCustomProperty.TryGetValue(contract.GetType(), out var properties);
            byte b = 0;
            foreach (var property in properties)
            {
                dictionary.Add(b++, property.Serialize(contract));
            }
            return dictionary;
        }

        public object FromDictionary(Type contractType, Dictionary<byte, object> parameters)
        {
            object contract = Activator.CreateInstance(contractType);

            byte b = 0;
            _contractCustomProperty.TryGetValue(contractType, out var properties);
            foreach (var property in properties)
            {
                if (parameters.TryGetValue(b++, out var input))
                {
                    property.Deserialize(contract, input);
                }
            }

            return contract;
        }

        public Dictionary<byte, object> ToDictionary<T>(T contract)
        {
            return ToDictionary(typeof(T), contract);
        }

        public T FromDictionary<T>(Dictionary<byte, object> parameters)
        {
            return (T) FromDictionary(typeof(T), parameters);
        }
    }
}

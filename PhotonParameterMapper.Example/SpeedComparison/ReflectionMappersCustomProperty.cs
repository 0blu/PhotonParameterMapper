using System;
using System.Reflection;
using PhotonParameterMapper.Example.CustomTypes;

namespace PhotonParameterMapper.Example.SpeedComparison
{
    class ReflectionMappersCustomProperty
    {
        public delegate object SerializeMethodDelegate(object obj);
        public delegate void DeserializeMethodDelegate(object contract, object input);

        public SerializeMethodDelegate Serialize;

        public DeserializeMethodDelegate Deserialize;

        private readonly FieldInfo _propertyInfo;

        public string Name => _propertyInfo.Name;

        public ReflectionMappersCustomProperty(FieldInfo info)
        {
            _propertyInfo = info;
            if (info.FieldType == typeof(FixPoint))
            {
                Serialize = SerializeFixpoint;
                Deserialize = DeserializeFixpoint;
                return;
            }

            if (info.FieldType == typeof(Guid))
            {
                Serialize = SerializeGuid;
                Deserialize = DeserializeGuid;
                return;
            }

            Serialize = SerializeMethod;
            Deserialize = DeserializeMethod;
        }
        
        private object SerializeFixpoint(object obj)
        {
            return ((FixPoint)_propertyInfo.GetValue(obj)).InternalValue;
        }

        private void DeserializeFixpoint(object output, object input)
        {
            _propertyInfo.SetValue(output, FixPoint.FromInternalValue((long)input));
        }

        private object SerializeGuid(object obj)
        {
            return ((Guid)_propertyInfo.GetValue(obj)).ToByteArray();
        }

        private void DeserializeGuid(object contract, object input)
        {
            _propertyInfo.SetValue(contract, new Guid((byte[])input));
        }

        private object SerializeMethod(object o)
        {
            return _propertyInfo.GetValue(o);
        }

        private void DeserializeMethod(object contract, object input)
        {
            _propertyInfo.SetValue(contract, input);
        }
    }
}

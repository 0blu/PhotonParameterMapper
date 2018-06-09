using PhotonParameterMapper.Core;
using PhotonParameterMapper.Example.CustomTypes;

namespace PhotonParameterMapper.Example.CustomFieldTypeBiserializers
{
    public class FixPointBiserializer : ICustomTypeBiserializer<FixPoint>
    {
        public object Serialize(FixPoint obj)
        {
            return obj.InternalValue;
        }

        public FixPoint Deserialize(object obj)
        {
            return FixPoint.FromInternalValue((long)obj);
        }
    }
}

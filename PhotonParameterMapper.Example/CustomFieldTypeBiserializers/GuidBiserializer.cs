using System;
using PhotonParameterMapper.Core;

namespace PhotonParameterMapper.Example.CustomFieldTypeBiserializers
{
    public class GuidBiserializer : ICustomTypeBiserializer<Guid>
    {
        public object Serialize(Guid obj)
        {
            return obj.ToByteArray();
        }

        public Guid Deserialize(object obj)
        {
            return new Guid((byte[])obj);
        }
    }
}

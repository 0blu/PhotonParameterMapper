using PhotonParameterMapper.Core;
using PhotonParameterMapper.Example.CustomTypes;

namespace PhotonParameterMapper.Example.CustomFieldTypeBiserializers
{
    public class Vector2Biserializer : ICustomTypeBiserializer<Vector2>
    {
        public object Serialize(Vector2 obj)
        {
            return obj.ToArray();
        }

        public Vector2 Deserialize(object obj)
        {
            return Vector2.FromArray((float[])obj);
        }
    }
}

using PhotonParameterMapper.Example.CustomTypes;

namespace PhotonParameterMapper.Example.ExampleContracts.Inheration
{
    public class ObjectSay : EventWithObjectId
    {
        public string Message;
        public ExampleEnum MessageType;

        public Vector2 FirstVector;
        public Vector2 SecondVector;
    }
}

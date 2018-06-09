using PhotonParameterMapper.Core;

namespace PhotonParameterMapper.Example.ExampleContracts.Inheration
{
    public class EventDataContract
    {
        [DataField(253, IsOptional = false)]
        public ExampleEventCode EventCode;
    }
}

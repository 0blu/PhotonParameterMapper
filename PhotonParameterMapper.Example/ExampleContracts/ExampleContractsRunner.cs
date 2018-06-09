using PhotonParameterMapper.Builder;
using PhotonParameterMapper.Core;
using PhotonParameterMapper.Example.CustomFieldTypeBiserializers;
using PhotonParameterMapper.Example.CustomTypes;
using PhotonParameterMapper.Example.ExampleContracts.Inheration;
using PhotonParameterMapper.Example.ExampleContracts.SimpleContracts;
using System;
using System.Collections.Generic;

namespace PhotonParameterMapper.Example.ExampleContracts
{
    public class ExampleContractsRunner
    {
        private APhotonMapper _photonMapper;

        public void RunExamples()
        {
            ISaveablePhotonMapperBuilder builder = PhotonMapperBuilderFactory.CreateSaveablePhotonMapper("TestAsm", "myFile.dll", "TestPrefix");

            builder.RegisterContract<SimpleContracts.SimpleContract>();
            builder.RegisterContract<SimpleContractWithCustomType>();
            builder.RegisterContract<ObjectSay>();

            builder.SaveAsDll();

            SimpleCustomTypeBiserializerRegistry biserializerRegistry = new SimpleCustomTypeBiserializerRegistry();
            biserializerRegistry.RegisterCustomTypeBiserializer<Guid, GuidBiserializer>();
            biserializerRegistry.RegisterCustomTypeBiserializer<FixPoint, FixPointBiserializer>();
            biserializerRegistry.RegisterCustomTypeBiserializer<Vector2, Vector2Biserializer>();

            _photonMapper = builder.ReleaseMapper(biserializerRegistry);

            RunSimpleContract();
            RunSimpleContractWithCustomType();
            RunObjectSay();
        }


        private void RunSimpleContract()
        {
            SimpleContract contract = new SimpleContract
            {
                TestDictionary = new Dictionary<string, string>(),
                Number = 2,
                Name = "Horst",
                AnEnum = ExampleEnum.Example1,
            };

            Dictionary<byte, object> parameters = _photonMapper.ToDictionary(contract);

            SimpleContract newContract = _photonMapper.FromDictionary<SimpleContract>(parameters);

            Console.WriteLine($"----{nameof(SimpleContract)}----");
            Console.WriteLine($"Number: '{newContract.Number}'");
            Console.WriteLine($"Name: '{newContract.Name}'");
            Console.WriteLine($"AnEnum: '{newContract.AnEnum}'");
        }
        
        private void RunSimpleContractWithCustomType()
        {
            SimpleContractWithCustomType contract = new SimpleContractWithCustomType
            {
                Value = FixPoint.FromIntegerValue(200),
                Position = new Vector2(13, 37)
            };

            Dictionary<byte, object> parameters = _photonMapper.ToDictionary(contract);
            SimpleContractWithCustomType newContract = _photonMapper.FromDictionary<SimpleContractWithCustomType>(parameters);

            Console.WriteLine($"----{nameof(SimpleContractWithCustomType)}----");
            Console.WriteLine($"Value: '{newContract.Value}'");
            Console.WriteLine($"Position: '{newContract.Position}'");
        }

        private void RunObjectSay()
        {
            ObjectSay contract = new ObjectSay
            {
                EventCode = ExampleEventCode.ObjectSay,
                ObjectId = 1234,
                Message = "Woof!",
                MessageType = ExampleEnum.Example1,
                FirstVector = Vector2.Zero,
                SecondVector = new Vector2(99.42f, 0.22f),
            };


            Dictionary<byte, object> parameters = _photonMapper.ToDictionary(contract);
            ObjectSay newContract = _photonMapper.FromDictionary<ObjectSay>(parameters);

            Console.WriteLine($"----{nameof(ObjectSay)}----");
            Console.WriteLine($"parameters[253] = {parameters[253]}");
            Console.WriteLine($"---");
            Console.WriteLine($"EventCode: '{newContract.EventCode}'");
            Console.WriteLine($"ObjectId: '{newContract.ObjectId}'");
            Console.WriteLine($"Message: '{newContract.Message}'");
            Console.WriteLine($"MessageType: '{newContract.MessageType}'");
            Console.WriteLine($"FirstVector: '{newContract.FirstVector}'");
            Console.WriteLine($"SecondVector: '{newContract.SecondVector}'");
        }
    }
}

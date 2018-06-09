using PhotonParameterMapper.Builder;
using PhotonParameterMapper.Core;
using PhotonParameterMapper.Example.CustomFieldTypeBiserializers;
using PhotonParameterMapper.Example.CustomTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using PhotonParameterMapper.Example.ExampleContracts;
using PhotonParameterMapper.Example.SpeedComparison;

namespace PhotonParameterMapper.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"########{nameof(ExampleContractsRunner)}########");
            var exampleContracts = new ExampleContractsRunner();
            exampleContracts.RunExamples();
            
            Console.WriteLine($"########{nameof(SpeedTestRunner)}########");
            var speedTest = new SpeedTestRunner();
            speedTest.RunSpeedTest();

            Console.WriteLine("Press any Key");
            Console.ReadLine();
        }
    }
}

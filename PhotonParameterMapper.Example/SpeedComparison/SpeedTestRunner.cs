using PhotonParameterMapper.Builder;
using PhotonParameterMapper.Core;
using PhotonParameterMapper.Example.CustomFieldTypeBiserializers;
using PhotonParameterMapper.Example.CustomTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PhotonParameterMapper.Example.SpeedComparison
{
    public class SpeedTestRunner
    {
        public const int WarmupIterations = 1_000;
        public const int TotalIterations = 1_000_000;

        private APhotonMapper _mapper;

        private ReflectionMapper _reflectionMapper;
        
        public void RunSpeedTest()
        {
            // To use this, you need to prebuild the assembly and then reference it in visual studio
            //PrepareTestPreBuildPhotonMapper();
            //TestPreBuildPhotonMapper();

            PrepareTestPhotonMapper();
            TestPhotonMapper();

            PrepareTestReflectionMapper();
            TestReflectionMapper();
        }
        
        /*
        private void PrepareTestPreBuildPhotonMapper()
        {
            ISaveablePhotonMapperBuilder builder = PhotonMapperBuilderFactory.CreateSaveableBiserializer("PreBuildedMapper", "PreBuildedMapper.dll", "PreBuildTestAssembly");

            builder.RegisterContract<SpeedComparisonContract>();

            SimpleCustomTypeBiserializerRegistry registry = new SimpleCustomTypeBiserializerRegistry();
            registry.RegisterCustomTypeBiserializer<Guid, GuidBiserializer>();
            registry.RegisterCustomTypeBiserializer<Vector2, Vector2Biserializer>();

            builder.SaveAsDll();
        }

        private void TestPreBuildPhotonMapper()
        {
            SimpleCustomTypeBiserializerRegistry registry = new SimpleCustomTypeBiserializerRegistry();
            registry.RegisterCustomTypeBiserializer<Guid, GuidBiserializer>();
            registry.RegisterCustomTypeBiserializer<Vector2, Vector2Biserializer>();

            APhotonMapper mapper = new PreBuildTestAssembly.PhotonMapper(registry);

            SpeedComparisonContract contract = CreateSpeedComparisonContract();

            Stopwatch sw = new Stopwatch();
            for (int i = 0; i < WarmupIterations; i++)
            {
                mapper.ToDictionary(contract);
            }
            sw.Restart();
            for (int i = 0; i < TotalIterations; i++)
            {
                mapper.ToDictionary(contract);
            }
            sw.Stop();
            Console.WriteLine($"PreBuildTestAssembly.ToDictionary: {sw.Elapsed.TotalMilliseconds}ms ({sw.Elapsed.TotalMilliseconds / TotalIterations}ms/iterations)");

            Dictionary<byte, object> parameters = mapper.ToDictionary(contract);
            for (int i = 0; i < WarmupIterations; i++)
            {
                mapper.FromDictionary<SpeedComparisonContract>(parameters);
            }
            sw.Restart();
            for (int i = 0; i < TotalIterations; i++)
            {
                mapper.FromDictionary<SpeedComparisonContract>(parameters);
            }
            Console.WriteLine($"PreBuildTestAssembly.FromDictionary: {sw.Elapsed.TotalMilliseconds}ms ({sw.Elapsed.TotalMilliseconds / TotalIterations}ms/iterations)");
            sw.Stop();
        }
        */

        private void PrepareTestPhotonMapper()
        {
            IPhotonMapperBuilder builder = PhotonMapperBuilderFactory.CreatePhotonMapper();

            builder.RegisterContract<SpeedComparisonContract>();

            SimpleCustomTypeBiserializerRegistry registry = new SimpleCustomTypeBiserializerRegistry();
            registry.RegisterCustomTypeBiserializer<Guid, GuidBiserializer>();
            registry.RegisterCustomTypeBiserializer<Vector2, Vector2Biserializer>();

            _mapper = builder.ReleaseMapper(registry);

        }

        private void TestPhotonMapper()
        {
            SpeedComparisonContract contract = CreateSpeedComparisonContract();

            Stopwatch sw = new Stopwatch();
            for (int i = 0; i < WarmupIterations; i++)
            {
                _mapper.ToDictionary(contract);
            }
            sw.Restart();
            for (int i = 0; i < TotalIterations; i++)
            {
                _mapper.ToDictionary(contract);
            }
            sw.Stop();
            Console.WriteLine($"PhotonMapper.ToDictionary: {sw.Elapsed.TotalMilliseconds}ms ({sw.Elapsed.TotalMilliseconds / TotalIterations}ms/iterations)");

            Dictionary<byte, object> parameters = _mapper.ToDictionary(contract);
            for (int i = 0; i < WarmupIterations; i++)
            {
                _mapper.FromDictionary<SpeedComparisonContract>(parameters);
            }
            sw.Restart();
            for (int i = 0; i < TotalIterations; i++)
            {
                _mapper.FromDictionary<SpeedComparisonContract>(parameters);
            }
            Console.WriteLine($"PhotonMapper.FromDictionary: {sw.Elapsed.TotalMilliseconds}ms ({sw.Elapsed.TotalMilliseconds / TotalIterations}ms/iterations)");
            sw.Stop();
        }

        private void PrepareTestReflectionMapper()
        {
            _reflectionMapper = new ReflectionMapper();
            _reflectionMapper.RegisterContract<SpeedComparisonContract>();
        }

        private void TestReflectionMapper()
        {
            SpeedComparisonContract contract = CreateSpeedComparisonContract();

            Stopwatch sw = new Stopwatch();
            for (int i = 0; i < WarmupIterations; i++)
            {
                _reflectionMapper.ToDictionary(contract);
            }
            sw.Restart();
            for (int i = 0; i < TotalIterations; i++)
            {
                _reflectionMapper.ToDictionary(contract);
            }
            sw.Stop();
            Console.WriteLine($"ReflectionMapper.ToDictionary: {sw.Elapsed.TotalMilliseconds}ms ({sw.Elapsed.TotalMilliseconds / TotalIterations}ms/iterations)");

            Dictionary<byte, object> parameters = _reflectionMapper.ToDictionary(contract);
            for (int i = 0; i < WarmupIterations; i++)
            {
                _reflectionMapper.FromDictionary<SpeedComparisonContract>(parameters);
            }
            sw.Restart();
            for (int i = 0; i < TotalIterations; i++)
            {
                _reflectionMapper.FromDictionary<SpeedComparisonContract>(parameters);
            }
            Console.WriteLine($"ReflectionMapper.FromDictionary: {sw.Elapsed.TotalMilliseconds}ms ({sw.Elapsed.TotalMilliseconds / TotalIterations}ms/iterations)");
            sw.Stop();
        }

        private SpeedComparisonContract CreateSpeedComparisonContract()
        {
            return new SpeedComparisonContract
            {
                Field1 = "TestString",
                Field2 = 2,
                Field3 = 43000000,
                Field4 = 50,
                Field5 = Guid.NewGuid(),
                Field10 = 23432424,
                Field11 = new Vector2(40, 23),
                Field12 = new Vector2(400, 253),
                Field13 = 30,
                Field14 = 500,
                Field15 = 90,
                Field16 = 90,
                Field17 = 90,
                Field18 = 90,
                Field20 = 90,
                Field21 = new [] {1, 2, 3},
                Field22 = new[] { 1, 2, 3 },
                Field23 = 90,
                Field24 = 90,
                Field25 = 90,
                Field26 = 90,
                Field27 = 90,
                Field28 = 90,
                Field29 = 90,
                Field30 = "Hello World",
                Field31 = Guid.NewGuid(),
                Field32 = true,
                Field33 = false,
                Field34 = false,
            };
        }
    }
}

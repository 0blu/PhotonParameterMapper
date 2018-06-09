using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace PhotonParameterMapper.Builder.Internal.Builder
{
    internal class InternalAssemblyBuilder
    {
        private readonly AssemblyBuilder _assemblyBuilder;
        private readonly ModuleBuilder _moduleBuilder;
        private readonly string _fileName;
        private readonly string _namespacePrefix;
        
        public InternalAssemblyBuilder(string assemblyName, string fileName = null, string namespacePrefix = "PhotonParameterMapper")
        {
            _namespacePrefix = namespacePrefix;
            _fileName = fileName;

            AppDomain currentDomain = AppDomain.CurrentDomain;
            AssemblyName name = new AssemblyName(assemblyName);

            if (_fileName != null)
            {
                _assemblyBuilder = currentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave);
                _moduleBuilder = _assemblyBuilder.DefineDynamicModule(_assemblyBuilder.GetName().Name, _fileName);
            }
            else
            {
                _assemblyBuilder = currentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
                _moduleBuilder = _assemblyBuilder.DefineDynamicModule(_assemblyBuilder.GetName().Name);
            }
        }


        public Type BuildMapper(IEnumerable<Type> contractTypes)
        {
            Dictionary<Type, Type> contractToContractMapperTypes = new Dictionary<Type, Type>();

            foreach (Type contractType in contractTypes)
            {
                contractToContractMapperTypes.Add(contractType, ContractMapperBuilder.BuildContractMapper(_moduleBuilder, _namespacePrefix, contractType));
            }

            return PhotonMapperTypeCreator.BuildMapper(_moduleBuilder, _namespacePrefix, contractToContractMapperTypes);
        }

        public void SaveAsDll()
        {
            _assemblyBuilder.Save(_fileName);
        }
    }
}

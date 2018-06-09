using PhotonParameterMapper.Builder.Internal.Builder;
using PhotonParameterMapper.Core;
using System;
using System.Collections.Generic;

namespace PhotonParameterMapper.Builder.Internal
{
    internal class PhotonMapperBuilder : ISaveablePhotonMapperBuilder
    {
        private Type _initializedType;
        private readonly HashSet<Type> _registerdContractTypes = new HashSet<Type>();
        private readonly InternalAssemblyBuilder _assemblyBuilder;

        public PhotonMapperBuilder()
        {
            _assemblyBuilder = new InternalAssemblyBuilder(Guid.NewGuid().ToString());
        }

        public PhotonMapperBuilder(string assemblyName, string fileName, string namespacePrefix)
        {
            _assemblyBuilder = new InternalAssemblyBuilder(assemblyName, fileName, namespacePrefix);
        }

        public void RegisterContract(Type type)
        {
            if (_initializedType != null)
            {
                throw new Exception("Mapper is already initialized and cannot be edited anymore");
            }

            if (!_registerdContractTypes.Add(type))
            {
                throw new ArgumentException("Type was already registerd");
            }
        }

        public void RegisterContract<T>()
        {
            RegisterContract(typeof(T));
        }

        private Type BuildMapper()
        {
            if (_initializedType == null)
            {
                _initializedType = _assemblyBuilder.BuildMapper(_registerdContractTypes);
            }
            return _initializedType;
        }

        public APhotonMapper ReleaseMapper(ICustomTypeBiserializerRegistry biserializerRegistry)
        {
            Type biserializerType = BuildMapper();

            return (APhotonMapper)Activator.CreateInstance(biserializerType, biserializerRegistry);
        }

        public void SaveAsDll()
        {
            BuildMapper();
            _assemblyBuilder.SaveAsDll();
        }
    }
}

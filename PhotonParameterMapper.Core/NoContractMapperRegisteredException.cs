using System;

namespace PhotonParameterMapper.Core
{
    /// <summary>
    /// Might be used by <see cref="ICustomTypeBiserializerRegistry"/>
    /// </summary>
    public class NoContractMapperRegisteredException : Exception
    {
        /// <summary>
        /// The contract type that was not found
        /// </summary>
        public readonly Type ContractType;

        /// <summary>
        /// </summary>
        /// <param name="contractType"></param>
        public NoContractMapperRegisteredException(Type contractType)
            : base($"No mapper for '{contractType.FullName}' registered")
        {
            ContractType = contractType;
        }
    }
}

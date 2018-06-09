using System;
using System.Collections.Generic;

namespace PhotonParameterMapper.Core
{
    /// <summary>
    /// Mapper to convert contracts to a parameters dictionary and vice versa  
    /// </summary>
    public abstract class APhotonMapper
    {
        /// <summary>
        /// A dictionary of all <see cref="ISingleContractMapper"/> instances
        /// </summary>
        protected Dictionary<Type, ISingleContractMapper> ContractMapperInstances = new Dictionary<Type, ISingleContractMapper>();

        /// <summary>
        /// Maps a contract to a dictionary
        /// <para />
        /// The order of the fields in the dictionary is consecutive (In regards of <see cref="DataFieldAttribute"/>)
        /// </summary>
        /// <param name="contractType">Type of the contract</param>
        /// <param name="contract">Contract instance</param>
        /// <returns>Dictionary with all field values of the contract</returns>
        public Dictionary<byte, object> ToDictionary(Type contractType, object contract)
        {
            if (!ContractMapperInstances.TryGetValue(contractType, out ISingleContractMapper mapper))
                throw new NoContractMapperRegisteredException(contractType);

            return mapper.ToDictionary(contract);
        }

        /// <summary>
        /// Maps a contract to a dictionary
        /// <para />
        /// The order of the fields in the dictionary is consecutive (In regards of <see cref="DataFieldAttribute"/>)
        /// </summary>
        /// <param name="contract">Contract instance</param>
        /// <typeparam name="T">Type of the contract</typeparam>
        /// <returns>Dictionary with all field values of the contract</returns>
        public Dictionary<byte, object> ToDictionary<T>(T contract)
        {
            return ToDictionary(typeof(T), contract);
        }

        /// <summary>
        /// Converts parametes to a contract
        /// </summary>
        /// <param name="targetContractType">Contract type of the new object</param>
        /// <param name="parameters">A dictionary with serialize field data</param>
        /// <returns>A new filled contract with all fields set</returns>
        /// <exception cref="FieldDeserializationException">Thrown when an error occured while deserializing a field</exception>
        /// <exception cref="NoContractMapperRegisteredException">Will be thrown the type is not registerd</exception>
        public object FromDictionary(Type targetContractType, Dictionary<byte, object> parameters)
        {
            if (!ContractMapperInstances.TryGetValue(targetContractType, out ISingleContractMapper mapper))
                throw new NoContractMapperRegisteredException(targetContractType);
            return mapper.FromDictionary(parameters);
        }

        /// <summary>
        /// Converts parametes to a contract
        /// </summary>
        /// <param name="parameters">A dictionary with serialize field data</param>
        /// <typeparam name="T">Contract type of the new object</typeparam>
        /// <returns>A new filled contract with all fields set</returns>
        /// <exception cref="FieldDeserializationException">Thrown when an error occured while deserializing a field</exception>
        /// <exception cref="NoContractMapperRegisteredException">Will be thrown the type is not registerd</exception>
        public T FromDictionary<T>(Dictionary<byte, object> parameters) where T : new()
        {
            return (T)FromDictionary(typeof(T), parameters);
        }
    }
}

using System.Collections.Generic;

namespace PhotonParameterMapper.Core
{
    /// <summary>
    /// Mapper to convert a single type of contract to a parameters dictionary and vice versa 
    /// </summary>
    public interface ISingleContractMapper
    {
        /// <summary>
        /// Converts a contract instance to a parameters dictionary
        /// </summary>
        /// <param name="contract">Contract instance</param>
        /// <returns>Dictionary with all fields serialized</returns>
        Dictionary<byte, object> ToDictionary(object contract);

        /// <summary>
        /// Converts a parameters dictionary to a contract instance
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        object FromDictionary(Dictionary<byte, object> parameters);
    }
}

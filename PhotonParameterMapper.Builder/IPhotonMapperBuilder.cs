using PhotonParameterMapper.Core;
using System;

namespace PhotonParameterMapper.Builder
{
    public interface IPhotonMapperBuilder
    {
        /// <summary>
        /// Registers a new type to the biserializer.
        /// </summary>
        /// <remarks>
        /// This step must be done before using the <see cref="IPhotonMapper.Serialize"/> or <see cref="IPhotonMapper.Deserialize"/> method
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when the type is already registerd</exception>
        /// <param name="type">The type to register</param>
        void RegisterContract(Type type);

        /// <summary>
        /// Registers a new type to the biserializer.
        /// </summary>
        /// <remarks>
        /// This step must be done before using the <see cref="IPhotonMapper.Serialize"/> or <see cref="IPhotonMapper.Deserialize"/> method
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when the type is already registerd</exception>
        /// <typeparam name="T">The type to register</typeparam> name="type">
        void RegisterContract<T>();

        /// <summary>
        /// Creates an instance of a biserializer
        /// </summary>
        /// <param name="biserializerRegistry">A biserializerRegistry that is being used to get <see cref="ICustomTypeBiserializer{T}"/> instances</param>
        /// <returns>A new biserializer instance</returns>
        APhotonMapper ReleaseMapper(ICustomTypeBiserializerRegistry biserializerRegistry);
    }
}

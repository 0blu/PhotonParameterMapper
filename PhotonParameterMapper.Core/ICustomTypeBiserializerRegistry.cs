namespace PhotonParameterMapper.Core
{
    /// <summary>
    /// Provider for <see cref="ICustomTypeBiserializer{T}"/>
    /// </summary>
    public interface ICustomTypeBiserializerRegistry
    {
        /// <summary>
        /// Provides a instances of <see cref="ICustomTypeBiserializer{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Biserializer instance</returns>
        ICustomTypeBiserializer<T> GetCustomTypeBiserializer<T>();
    }
}

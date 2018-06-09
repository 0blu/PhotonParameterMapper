namespace PhotonParameterMapper.Core
{
    /// <summary>
    /// Serializes and Deserializes custom types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICustomTypeBiserializer<T>
    {
        /// <summary>
        /// Serializes an object
        /// </summary>
        /// <param name="obj">A instance of T</param>
        /// <returns>A serialized instance of T</returns>
        object Serialize(T obj);

        /// <summary>
        /// Deserialize an object
        /// </summary>
        /// <param name="obj">The object that should be deserialize</param>
        /// <returns>A instance of T</returns>
        T Deserialize(object obj);
    }
}

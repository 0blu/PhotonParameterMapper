using PhotonParameterMapper.Builder.Internal;

namespace PhotonParameterMapper.Builder
{
    /// <summary>
    /// Factory to create <see cref="IPhotonMapperBuilder"/>
    /// </summary>
    public class PhotonMapperBuilderFactory
    {
        /// <summary>
        /// Creates a simple <see cref="IPhotonMapperBuilder"/>
        /// </summary>
        /// <returns></returns>
        public static IPhotonMapperBuilder CreatePhotonMapper()
        {
            return new PhotonMapperBuilder();
        }

        /// <summary>
        /// Creates a <see cref="ISaveablePhotonMapperBuilder"/> that can save the mapper to an assembly
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="fileName"></param>
        /// <param name="namespacePrefix"></param>
        /// <returns></returns>
        public static ISaveablePhotonMapperBuilder CreateSaveablePhotonMapper(string assemblyName, string fileName, string namespacePrefix)
        {
            return new PhotonMapperBuilder(assemblyName, fileName, namespacePrefix);
        }
    }
}

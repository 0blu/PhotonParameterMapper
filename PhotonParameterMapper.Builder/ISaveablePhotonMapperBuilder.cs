namespace PhotonParameterMapper.Builder
{
    public interface ISaveablePhotonMapperBuilder : IPhotonMapperBuilder
    {
        /**
         * Saves the dll as a file to the given location
         */
        void SaveAsDll();
    }
}

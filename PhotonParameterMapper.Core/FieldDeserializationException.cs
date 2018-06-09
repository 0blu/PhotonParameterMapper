using System;

namespace PhotonParameterMapper.Core
{
    /// <summary>
    /// An exception that is thrown when a deserialization of a field failed
    /// </summary>
    public class FieldDeserializationException : Exception
    {
        /// <summary>
        /// The index of that field that failed. (In regards of <see cref="DataFieldAttribute"/>)
        /// </summary>
        public readonly byte FieldIndex;

        /// <summary>
        /// Should not be called manually, constructor is used by a contract mapper
        /// </summary>
        /// <param name="internalException">internal exception</param>
        /// <param name="fieldIndex">fieldIndex that failed</param>
        public FieldDeserializationException(Exception internalException, byte fieldIndex)
            : base($"Failed to deserialization field at index {fieldIndex}", internalException)
        {
            FieldIndex = fieldIndex;
        }
    }
}

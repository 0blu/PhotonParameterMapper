using System;

namespace PhotonParameterMapper.Core
{
    /// <summary>
    /// Attribute to define a data field in a contract.
    /// <para/>
    /// If no attribute is set the field index will increase
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DataFieldAttribute : Attribute
    {
        /// <summary>
        /// Is set when a custom field index is set
        /// </summary>
        public bool IsCustomFieldIndexSet { get; private set; }

        private byte _fieldIndex;

        /// <summary>
        /// The index of the field.
        /// <para/>
        /// Is only valid if <see cref="IsCustomFieldIndexSet"/> is true
        /// </summary>
        public byte FieldIndex
        {
            get => _fieldIndex;
            private set
            {
                IsCustomFieldIndexSet = true;
                _fieldIndex = value;
            }
        }

        /// <summary>
        /// True whenever the parameter is optional.
        /// <para/>
        /// Default is true
        /// <para/>
        /// Usefull for paramters that are read manually and always have to be in the paramters dictionary of <see cref="APhotonMapper"/>
        /// </summary>
        public bool IsOptional { get; set; } = true;

        /// <summary>
        /// Defines a field in a contract.
        /// <para/>
        /// This is not required
        /// </summary>
        public DataFieldAttribute()
        {
        }

        /// <summary>
        /// Sets a custom field index for <see cref="APhotonMapper"/>
        /// <para/>
        /// When using this, the field counter will not increase
        /// </summary>
        /// <param name="index">The index</param>
        public DataFieldAttribute(byte index)
        {
            FieldIndex = index;
        }
    }
}

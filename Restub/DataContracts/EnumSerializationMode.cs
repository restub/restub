namespace Restub.DataContracts
{
    /// <summary>
    /// Represents enumeration serialization modes.
    /// </summary>
    public enum EnumSerializationMode
    {
        /// <summary>
        /// Serialize enum values as numbers
        /// unless enum is marked as DataContract,
        /// in which case serialize values as strings.
        /// </summary>
        Auto = 0,

        /// <summary>
        /// Serialize enum values as numbers.
        /// </summary>
        Number = 1,

        /// <summary>
        /// Serialize enum values as strings.
        /// </summary>
        String = 2,
    }
}

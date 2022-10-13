using System.Runtime.Serialization;

namespace Restub.DataContracts
{
    /// <summary>
    /// Stub error response DTO implementing <see cref="IHasErrors"/> interface.
    /// </summary>
    [DataContract]
    public class ErrorResponse : IHasErrors
    {
        /// <inheritdocs/>
        public bool HasErrors() => false;

        /// <inheritdocs/>
        public string GetErrorMessage() => string.Empty;
    }
}

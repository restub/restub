using System.Collections.Generic;

namespace Restub.DataContracts
{
    /// <summary>
    /// Interface for REST responses containing errors.
    /// </summary>
    public interface IHasErrors
    {
        /// <summary>
        /// Gets the error message.
        /// </summary>
        bool HasErrors();

        /// <summary>
        /// Gets the error message.
        /// </summary>
        string GetErrorMessage();
    }
}

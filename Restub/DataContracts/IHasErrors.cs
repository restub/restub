using System.Collections.Generic;

namespace Restub.DataContracts
{
    /// <summary>
    /// Interface for REST responses containing errors.
    /// </summary>
    public interface IHasErrors
    {
        /// <summary>
        /// Returns true if the object has errors.
        /// </summary>
        bool HasErrors();

        /// <summary>
        /// Gets user-readable error message.
        /// </summary>
        string GetErrorMessage();
    }
}

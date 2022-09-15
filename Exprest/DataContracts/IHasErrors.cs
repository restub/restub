using System.Collections.Generic;

namespace Exprest.DataContracts
{
    /// <summary>
    /// Interface for REST responses containing error collections.
    /// </summary>
    public interface IHasErrors
    {
        /// <summary>
        /// Gets the errors.
        /// </summary>
        IEnumerable<Error> GetErrors();
    }
}

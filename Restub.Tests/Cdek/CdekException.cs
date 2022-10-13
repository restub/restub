using System;
using System.Net;

namespace Restub.Tests.Cdek
{
    /// <summary>
    /// Sample CDEK client exception class.
    /// </summary>
    public class CdekException : RestubException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CdekException"/> class.
        /// </summary>
        /// <param name="code">Http status code, see <see cref="HttpStatusCode"/>.</param>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception, if any.</param>
        public CdekException(HttpStatusCode code, string message, Exception innerException = null)
            : base(code, message, innerException)
        {
        }
    }
}

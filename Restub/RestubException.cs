using System;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using Restub.DataContracts;

namespace Restub
{
    /// <summary>
    /// Base REST exception class.
    /// </summary>
    [Serializable]
    public class RestubException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestubException"/> class.
        /// </summary>
        /// <param name="code">HTTP status code.</param>
        /// <param name="message">Error message.</param>
        /// <param name="errorResponse"><see cref="ErrorResponse"/> instance, if available.</param>
        /// <param name="innerException">Inner <see cref="Exception"/> instance.</param>
        public RestubException(HttpStatusCode code, string message, Exception innerException)
            : base(GetMessage(code, message), innerException)
        {
            StatusCode = code;
        }

        /// <summary>
        /// HTTP status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        private static string GetMessage(HttpStatusCode code, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                return message;
            }

            return code.ToString();
        }

        /// <inheritdoc/>
        protected RestubException(SerializationInfo info, StreamingContext context)
        {
            StatusCode = (HttpStatusCode)info.GetInt32("Code");
        }

        /// <inheritdoc/>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Code", (int)StatusCode);
        }
    }
}

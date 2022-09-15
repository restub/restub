using System;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using Exprest.DataContracts;

namespace Exprest
{
    /// <summary>
    /// Base exception class.
    /// </summary>
    [Serializable]
    public class ExprestException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExprestException"/> class.
        /// </summary>
        /// <param name="code">HTTP status code.</param>
        /// <param name="message">Error message.</param>
        /// <param name="errorResponse"><see cref="ErrorResponse"/> instance, if available.</param>
        /// <param name="innerException">Inner <see cref="Exception"/> instance.</param>
        public ExprestException(HttpStatusCode code, string message, IHasErrors errorResponse, Exception innerException)
            : base(GetMessage(code, message), innerException)
        {
            StatusCode = code;
            ErrorResponse = new ErrorResponse
            {
                Errors = (errorResponse?.GetErrors() ?? Enumerable.Empty<Error>()).ToList(),
            };
        }

        private static string GetMessage(HttpStatusCode code, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                return message;
            }

            return code.ToString();
        }

        /// <inheritdoc/>
        protected ExprestException(SerializationInfo info, StreamingContext context)
        {
            StatusCode = (HttpStatusCode)info.GetInt32("Code");
        }

        /// <summary>
        /// HTTP status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// <see cref="ErrorResponse"/> instance returned by server.
        /// </summary>
        public ErrorResponse ErrorResponse { get; set; }

        /// <inheritdoc/>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Code", (int)StatusCode);
        }
    }
}

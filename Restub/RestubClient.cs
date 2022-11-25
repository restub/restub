using System;
using System.Linq;
using System.Text;
using System.Xml;
using RestSharp;
using RestSharp.Authenticators;
using Restub.DataContracts;
using Restub.Toolbox;

namespace Restub
{
    /// <summary>
    /// Base class for implementing REST API clients.
    /// </summary>
    public abstract partial class RestubClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestubClient"/> class.
        /// </summary>
        /// <param name="baseUrl">Base API endpoint.</param>
        /// <param name="credentials">Credentials.</param>
        public RestubClient(string baseUrl, Credentials credentials = null)
            : this(new RestClient(baseUrl), credentials)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestubClient"/> class.
        /// </summary>
        /// <param name="сlient">REST API client.</param>
        /// <param name="credentials">Credentials.</param>
        public RestubClient(IRestClient сlient, Credentials credentials = null)
        {
            Credentials = credentials;
            Serializer = CreateSerializer();

            // Set up REST client
            Client = сlient;
            Client.Authenticator = GetAuthenticator();
            Client.Encoding = GetEncoding();
            Client.ThrowOnDeserializationError = false;
            Client.UseSerializer(() => Serializer);
        }

        /// <summary>
        /// Gets the encoding used by REST service.
        /// </summary>
        /// <returns>Encoding for REST messages.</returns>
        protected virtual Encoding GetEncoding() => Encoding.UTF8;

        /// <summary>
        /// Gets or creates the authenticator.
        /// If derived class implements <see cref="IAuthenticator"/> interface, returns itself.
        /// If authentication is not required, returns null.
        /// </summary>
        /// <returns>Authenticator for REST requests, or null.</returns>
        protected virtual IAuthenticator GetAuthenticator() =>
            this as IAuthenticator;

        /// <summary>
        /// Creates the serializer for the <see cref="Serializer"/> property.
        /// </summary>
        /// <returns>Serializer for REST messages.</returns>
        protected virtual IRestubSerializer CreateSerializer() =>
            new NewtonsoftSerializer();

        /// <summary>
        /// Gets the library name.
        /// </summary>
        public virtual string LibraryName =>
            $"restub v{typeof(RestubClient).GetAssemblyVersion()}";

        /// <summary>
        /// Gets the library version.
        /// </summary>
        public virtual string LibraryVersion =>
            GetType().GetAssemblyVersion();

        /// <summary>
        /// Gets the REST API client.
        /// </summary>
        public IRestClient Client { get; }

        /// <summary>
        /// Gets the credentials.
        /// </summary>
        public Credentials Credentials { get; }

        /// <summary>
        /// Gets the serializer.
        /// </summary>
        public IRestubSerializer Serializer { get; }

        private void PrepareRequest(IRestRequest request, string apiMethodName)
        {
            // use request parameters to store additional properties, not really used by the requests
            request.AddParameter(ApiTimestampParameterName, DateTime.Now.Ticks, ParameterType.UrlSegment);
            request.AddParameter(ApiTickCountParameterName, Environment.TickCount.ToString(), ParameterType.UrlSegment);
            request.AddHeaderIfNotEmpty(ApiClientNameHeaderName, LibraryName);
            request.AddHeaderIfNotEmpty(ApiMethodNameHeaderName, apiMethodName);

            // trace requests and responses
            if (Tracer != null)
            {
                request.OnBeforeRequest = http => Trace(http, request);
                request.OnBeforeDeserialization = resp => Trace(resp);
            }
        }

        /// <summary>
        /// Checks if the typed response failed and throws the exception.
        /// </summary>
        /// <typeparam name="T">Typed response return type.</typeparam>
        /// <param name="response">Rest response.</param>
        protected virtual void ThrowOnFailure<T>(IRestResponse<T> response)
        {
            // if response is successful, but it has errors, treat is as failure
            if (response.Data is IHasErrors hasErrors && hasErrors.HasErrors())
            {
                ThrowOnFailure(response, hasErrors);
                return;
            }

            ThrowOnFailure(response as IRestResponse);
        }

        /// <summary>
        /// Checks if the untyped response failed and throws the exception.
        /// </summary>
        /// <param name="response">Rest response.</param>
        protected virtual void ThrowOnFailure(IRestResponse response)
        {
            if (!response.IsSuccessful)
            {
                // try to find the non-empty error message
                var errorMessage = response.ErrorMessage;
                var contentMessage = response.Content;
                var errorResponse = default(IHasErrors);
                if (response.ContentType != null)
                {
                    // Text/plain;charset=UTF-8 => text/plain
                    var contentType = response.ContentType.ToLower().Trim();
                    var semicolonIndex = contentType.IndexOf(';');
                    if (semicolonIndex >= 0)
                    {
                        contentType = contentType.Substring(0, semicolonIndex).Trim();
                    }

                    // Try to deserialize error response DTO
                    if (Serializer.SupportedContentTypes.Contains(contentType))
                    {
                        errorResponse = TryDeserializeErrorResponse(response);
                        contentMessage = GetErrorMessage(errorResponse);
                    }
                    else if (response.ContentType.ToLower().Contains("html"))
                    {
                        // Try to parse HTML
                        contentMessage = HtmlHelper.ExtractText(response.Content);
                    }
                    else
                    {
                        // Return content as is assuming it's text/plain
                        contentMessage = response.Content;
                    }
                }

                // HTML->XML deserialization errors are meaningless
                if (response.ErrorException is XmlException && errorMessage == response.ErrorException.Message)
                {
                    errorMessage = contentMessage;
                }

                // JSON deserialiation exception is meaningless
                if (response.ErrorException is Newtonsoft.Json.JsonSerializationException && 
                    errorMessage == response.ErrorException.Message &&
                    !string.IsNullOrWhiteSpace(contentMessage))
                {
                    errorMessage = contentMessage;
                }

                // empty error message is meaningless
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = contentMessage;
                }

                // finally, throw it
                throw CreateException(response, errorMessage, errorResponse);
            }
        }

        /// <summary>
        /// Checks if the response is failed and throws an exception.
        /// </summary>
        /// <param name="response">The response to check.</param>
        /// <param name="errorResponse">Deserialized error response supporting <see cref="IHasErrors"/> interface, optional.</param>
        protected virtual void ThrowOnFailure(IRestResponse response, IHasErrors errorResponse)
        {
            // if a response has errors, treat it as a failure
            if (errorResponse?.HasErrors() ?? false)
            {
                var errorMessage = GetErrorMessage(errorResponse);
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = response.ErrorMessage;
                }

                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = response.Content;
                }

                throw CreateException(response, errorMessage, errorResponse);
            }
        }

        /// <summary>
        /// Tries to deserialize JSON error response if REST API has single error response for all failures.
        /// Should never rethrow exceptions, returns null if response cannot be deserialized.
        /// </summary>
        /// <param name="response">Rest response to deserialize.</param>
        private IHasErrors TryDeserializeErrorResponse(IRestResponse response)
        {
            try
            {
                return DeserializeErrorResponse(response);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Deserialize JSON error response if REST API has single error response for all failures.
        /// </summary>
        /// <param name="response">Rest response to deserialize.</param>
        protected virtual IHasErrors DeserializeErrorResponse(IRestResponse response) =>
            Serializer.Deserialize<ErrorResponse>(response);

        /// <summary>
        /// Creates the API-specific exception class.
        /// </summary>
        /// <param name="res">Rest response.</param>
        /// <param name="msg">Error message.</param>
        /// <param name="errors"><see cref="IHasErrors"/> instance containing error details.</param>
        /// <returns>Exception to be thrown.</returns>
        protected virtual Exception CreateException(IRestResponse res, string msg, IHasErrors errors) =>
            new RestubException(res.StatusCode, msg, res.ErrorException)
            {
                ErrorResponseText = res.Content,
            };

        internal static string GetErrorMessage(IHasErrors errorResponse) =>
            errorResponse?.GetErrorMessage() ?? string.Empty;

        private void AddRequestBody(IRestRequest request, object body)
        {
            // don't check if body is null, add it anyway to init Content-type header
            if (request == null ||
                request.Method == Method.GET ||
                request.Method == Method.HEAD ||
                request.Method == Method.OPTIONS)
            {
                return;
            }

            // TODO: check if RestSharp itself already handles string bodies
            if (body is string strBody)
            {
                var contentType = "application/json";
                if (request.RequestFormat == DataFormat.Xml)
                {
                    contentType = "application/xml";
                }
                else if (request.RequestFormat != DataFormat.Json)
                {
                    contentType = "text/plain";
                }

                request.AddParameter(string.Empty, strBody, contentType, ParameterType.RequestBody);
            }
            else
            {
                request.AddJsonBody(body);
            }
        }
    }
}

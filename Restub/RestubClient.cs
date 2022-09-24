using System;
using System.Linq;
using System.Text;
using System.Xml;
using RestSharp;
using RestSharp.Serialization;
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
        public RestubClient(IRestClient сlient, Credentials credentials)
        {
            Credentials = credentials;
            Serializer = CreateSerializer();

            // Set up REST client
            Client = сlient;
            Client.Authenticator = CreateAuthenticator();
            Client.Encoding = Encoding.UTF8;
            Client.ThrowOnDeserializationError = false;
            Client.UseSerializer(() => Serializer);
        }

        /// <summary>
        /// When overridden in the derived class, creates the authenticator.
        /// </summary>
        /// <returns>Authenticator for REST requests, or null.</returns>
        protected virtual Authenticator CreateAuthenticator() => null;

        /// <summary>
        /// Creates the serializer.
        /// </summary>
        /// <returns>Serializer for REST messages.</returns>
        protected virtual IRestSerializer CreateSerializer() =>
            new NewtonsoftSerializer();

        /// <summary>
        /// Gets the library name.
        /// </summary>
        public virtual string LibraryName =>
            $"restub v{typeof(RestubClient).GetAssemblyVersion()}";

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
        public IRestSerializer Serializer { get; }

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

        protected virtual void ThrowOnFailure(IRestResponse response)
        {
            if (!response.IsSuccessful)
            {
                // try to find the non-empty error message
                var errorMessage = response.ErrorMessage;
                var contentMessage = response.Content;
                var errorResponse = default(ErrorResponse);
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
                        errorResponse = Serializer.Deserialize<ErrorResponse>(response);
                        contentMessage = string.Join(". ", errorResponse?.Errors?.Select(e => e.Message) ?? new[] { string.Empty }
                            .Distinct()
                            .Where(m => !string.IsNullOrWhiteSpace(m)));
                    }
                    else if (response.ContentType.ToLower().Contains("html"))
                    {
                        // Try to parse HTML
                        contentMessage = HtmlHelper.ExtractText(response.Content);
                    }
                    else
                    {
                        // Return as is assuming text/plain content
                        contentMessage = response.Content;
                    }
                }

                // HTML->XML deserialization errors are meaningless
                if (response.ErrorException is XmlException && errorMessage == response.ErrorException.Message)
                {
                    errorMessage = contentMessage;
                }

                // JSON deserialiation exception is meaningless
                if (response.ErrorException is Newtonsoft.Json.JsonSerializationException && errorMessage == response.ErrorException.Message)
                {
                    errorMessage = contentMessage;
                }

                // empty error message is meaningless
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = contentMessage;
                }

                // finally, throw it
                ThrowException(response, errorMessage, errorResponse);
            }
        }

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

                ThrowException(response, errorMessage, errorResponse);
            }
        }

        protected virtual void ThrowException(IRestResponse response, string errorMessage, IHasErrors errorResponse)
        {
            throw new RestubException(response.StatusCode, errorMessage, response.ErrorException);
        }

        internal static string GetErrorMessage(IHasErrors errorResponse) =>
            errorResponse?.GetErrorMessage() ?? string.Empty;

        private void AddRequestBody(IRestRequest request, object body)
        {
            if (request == null || body == null)
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

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using Restub.DataContracts;
using Restub.Toolbox;
using RestSharp;

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
        public RestubClient(string baseUrl, Credentials credentials)
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
            Serializer = new NewtonsoftSerializer();

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
        /// <returns>Authenticator for REST request.</returns>
        protected abstract Authenticator CreateAuthenticator();

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
        public NewtonsoftSerializer Serializer { get; }

        private void PrepareRequest(IRestRequest request, string apiMethodName)
        {
            // use request parameters to store additional properties, not really used by the requests
            request.AddParameter(ApiTimestampParameterName, DateTime.Now.Ticks, ParameterType.UrlSegment);
            request.AddParameter(ApiTickCountParameterName, Environment.TickCount.ToString(), ParameterType.UrlSegment);
            if (!string.IsNullOrWhiteSpace(apiMethodName))
            {
                request.AddHeader(ApiMethodNameHeaderName, apiMethodName);
            }

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
            if (response.Data is IHasErrors hasErrors && hasErrors.GetErrors().Any())
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
            if (errorResponse?.GetErrors()?.Any() ?? false)
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
            string.Join(". ", errorResponse?.GetErrors()?.Select(e => e.Message) ?? new[] { string.Empty }
                .Distinct()
                .Where(m => !string.IsNullOrWhiteSpace(m)));

        /// <summary>
        /// Executes the given request and checks the result.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="request">The request to execute.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        internal T Execute<T>(IRestRequest request, [CallerMemberName] string apiMethodName = null)
        {
            PrepareRequest(request, apiMethodName);
            var response = Client.Execute<T>(request);

            // handle REST exceptions
            ThrowOnFailure(response);
            return response.Data;
        }

        /// <summary>
        /// Executes the given request and checks the result.
        /// </summary>
        /// <param name="request">The request to execute.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        internal void Execute(IRestRequest request, [CallerMemberName] string apiMethodName = null)
        {
            PrepareRequest(request, apiMethodName);
            var response = Client.Execute(request);

            // there is no body deserialization step, so we need to trace
            Trace(response);
            ThrowOnFailure(response);
        }

        /// <summary>
        /// Executes the given request and checks the result.
        /// </summary>
        /// <param name="request">The request to execute.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        internal string ExecuteString(IRestRequest request, [CallerMemberName] string apiMethodName = null)
        {
            PrepareRequest(request, apiMethodName);
            var response = Client.Execute(request);

            // there is no body deserialization step, so we need to trace
            Trace(response);
            ThrowOnFailure(response);
            return response.Content;
        }

        /// <summary>
        /// Performs GET request.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Resource url.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public T Get<T>(string url, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null)
        {
            var request = new RestRequest(url, Method.GET, DataFormat.Json);
            initRequest?.Invoke(request);
            return Execute<T>(request, apiMethodName);
        }

        /// <summary>
        /// Performs GET request and returns a string.
        /// </summary>
        /// <param name="url">Resource url.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public string Get(string url, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null)
        {
            var request = new RestRequest(url, Method.GET, DataFormat.Json);
            initRequest?.Invoke(request);
            return ExecuteString(request, apiMethodName);
        }

        /// <summary>
        /// Performs POST request.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Resource url.</param>
        /// <param name="body">Request body, to be serialized as JSON.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public T Post<T>(string url, object body, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null)
        {
            var request = new RestRequest(url, Method.POST, DataFormat.Json);
            request.AddJsonBody(body);
            initRequest?.Invoke(request);
            return Execute<T>(request, apiMethodName);
        }

        /// <summary>
        /// Performs POST request.
        /// </summary>
        /// <param name="url">Resource url.</param>
        /// <param name="body">Request body, to be serialized as JSON.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public void Post(string url, object body, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null)
        {
            var request = new RestRequest(url, Method.POST, DataFormat.Json);
            request.AddJsonBody(body);
            initRequest?.Invoke(request);
            Execute(request, apiMethodName);
        }

        /// <summary>
        /// Performs PUT request.
        /// </summary>
        /// <param name="url">Resource url.</param>
        /// <param name="body">Request body, to be serialized as JSON.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public void Put(string url, object body, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null)
        {
            var request = new RestRequest(url, Method.PUT, DataFormat.Json);
            request.AddJsonBody(body);
            initRequest?.Invoke(request);
            Execute(request, apiMethodName);
        }

        /// <summary>
        /// Performs PUT request.
        /// </summary>
        /// <param name="url">Resource url.</param>
        /// <param name="body">Request body, serialized as string.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public void Put(string url, string body, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null)
        {
            var request = new RestRequest(url, Method.PUT, DataFormat.None);
            request.AddParameter(string.Empty, body, ParameterType.RequestBody);
            initRequest?.Invoke(request);
            Execute(request, apiMethodName);
        }

        /// <summary>
        /// Performs DELETE request.
        /// </summary>
        /// <param name="url">Resource url.</param>
        /// <param name="body">Request body, serialized as string.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public T Delete<T>(string url, object body, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null)
        {
            var request = new RestRequest(url, Method.DELETE, DataFormat.Json);
            if (body != null)
            {
                request.AddJsonBody(body);
            }

            initRequest?.Invoke(request);
            return Execute<T>(request, apiMethodName);
        }

        /// <summary>
        /// Performs DELETE request.
        /// </summary>
        /// <param name="url">Resource url.</param>
        /// <param name="body">Request body, serialized as string.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public void Delete(string url, object body, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null)
        {
            var request = new RestRequest(url, Method.DELETE, DataFormat.Json);
            if (body != null)
            {
                request.AddJsonBody(body);
            }

            initRequest?.Invoke(request);
            Execute(request, apiMethodName);
        }
    }
}
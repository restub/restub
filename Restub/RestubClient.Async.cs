using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RestSharp;

namespace Restub
{
    /// <remarks>
    /// Stub REST client, asynchronous methods.
    /// </remarks>
    public abstract partial class RestubClient
    {
        /// <summary>
        /// Executes the given request and checks the result synchronously.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="request">The request to execute.</param>
        /// <param name="body">Optional request body.</param>
        /// <param name="initRequest">IRestRequest initialization callback.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        internal async Task<T> ExecuteAsync<T>(IRestRequest request, object body = null,
            Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null)
        {
            AddRequestBody(request, body);
            initRequest?.Invoke(request);
            PrepareRequest(request, apiMethodName);

            // special treatment for the string requests
            if (typeof(T) == typeof(string) || typeof(T) == typeof(byte[]))
            {
                BeforeExecute(request);
                var response = await Client.ExecuteAsync(request);
                AfterExecute(request, response);

                // there is no body deserialization step, so we need to trace explicitly
                Trace(response);
                ThrowOnFailure(response);

                if (typeof(T) == typeof(string))
                {
                    return (T)(object)response.Content;
                }
                else // typeof(T) == typeof(byte[])
                {
                    return (T)(object)response.RawBytes;
                }
            }
            else
            {
                BeforeExecute(request);
                var response = await Client.ExecuteAsync<T>(request);
                AfterExecute(request, response);

                // handle REST exceptions
                ThrowOnFailure(response);
                return response.Data;
            }
        }

        /// <summary>
        /// Performs DELETE request.
        /// </summary>
        /// <param name="url">Resource url.</param>
        /// <param name="body">Request body, to be serialized as JSON or added as is if it's a string.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public Task<T> DeleteAsync<T>(string url, object body, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null) =>
            ExecuteAsync<T>(new RestRequest(url, Method.DELETE, DataFormat.Json), body, initRequest, apiMethodName);

        /// <summary>
        /// Performs GET request.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Resource url.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public Task<T> GetAsync<T>(string url, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null) =>
            ExecuteAsync<T>(new RestRequest(url, Method.GET, DataFormat.Json), null, initRequest, apiMethodName);

        /// <summary>
        /// Performs HEAD request.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Resource url.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public Task<T> HeadAsync<T>(string url, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null) =>
            ExecuteAsync<T>(new RestRequest(url, Method.HEAD, DataFormat.Json), null, initRequest, apiMethodName);

        /// <summary>
        /// Performs OPTIONS request.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Resource url.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public Task<T> OptionsAsync<T>(string url, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null) =>
            ExecuteAsync<T>(new RestRequest(url, Method.OPTIONS, DataFormat.Json), null, initRequest, apiMethodName);

        /// <summary>
        /// Performs PATCH request.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Resource url.</param>
        /// <param name="body">Request body, to be serialized as JSON or added as is if it's a string.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public Task<T> PatchAsync<T>(string url, object body, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null) =>
            ExecuteAsync<T>(new RestRequest(url, Method.PATCH, DataFormat.Json), body, initRequest, apiMethodName);

        /// <summary>
        /// Performs POST request.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Resource url.</param>
        /// <param name="body">Request body, to be serialized as JSON or added as is if it's a string.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public Task<T> PostAsync<T>(string url, object body, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null) =>
            ExecuteAsync<T>(new RestRequest(url, Method.POST, DataFormat.Json), body, initRequest, apiMethodName);

        /// <summary>
        /// Performs PUT request.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Resource url.</param>
        /// <param name="body">Request body, to be serialized as JSON or added as is if it's a string.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public Task<T> PutAsync<T>(string url, object body, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null) =>
            ExecuteAsync<T>(new RestRequest(url, Method.PUT, DataFormat.Json), body, initRequest, apiMethodName);
    }
}
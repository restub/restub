using System;
using System.Runtime.CompilerServices;
using RestSharp;

namespace Restub
{
    /// <remarks>
    /// Stub REST client, synchronous methods.
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
        internal T Execute<T>(IRestRequest request, object body = null,
            Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null)
        {
            AddRequestBody(request, body);
            initRequest?.Invoke(request);
            PrepareRequest(request, apiMethodName);

            // special treatment for the string and byte[] requests
            if (typeof(T) == typeof(string) || typeof(T) == typeof(byte[]))
            {
                BeforeExecute(request);
                var response = Client.Execute(request);
                AfterExecute(response);

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
                var response = Client.Execute<T>(request);
                AfterExecute(response);

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
        public T Delete<T>(string url, object body, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null) =>
            Execute<T>(new RestRequest(url, Method.DELETE, DataFormat.Json), body, initRequest, apiMethodName);

        /// <summary>
        /// Performs GET request.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Resource url.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public T Get<T>(string url, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null) =>
            Execute<T>(new RestRequest(url, Method.GET, DataFormat.Json), null, initRequest, apiMethodName);

        /// <summary>
        /// Performs HEAD request.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Resource url.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public T Head<T>(string url, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null) =>
            Execute<T>(new RestRequest(url, Method.HEAD, DataFormat.Json), null, initRequest, apiMethodName);

        /// <summary>
        /// Performs OPTIONS request.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Resource url.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public T Options<T>(string url, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null) =>
            Execute<T>(new RestRequest(url, Method.OPTIONS, DataFormat.Json), null, initRequest, apiMethodName);

        /// <summary>
        /// Performs PATCH request.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Resource url.</param>
        /// <param name="body">Request body, to be serialized as JSON or added as is if it's a string.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public T Patch<T>(string url, object body, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null) =>
            Execute<T>(new RestRequest(url, Method.PATCH, DataFormat.Json), body, initRequest, apiMethodName);

        /// <summary>
        /// Performs POST request.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Resource url.</param>
        /// <param name="body">Request body, to be serialized as JSON or added as is if it's a string.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public T Post<T>(string url, object body, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null) =>
            Execute<T>(new RestRequest(url, Method.POST, DataFormat.Json), body, initRequest, apiMethodName);

        /// <summary>
        /// Performs PUT request.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Resource url.</param>
        /// <param name="body">Request body, to be serialized as JSON or added as is if it's a string.</param>
        /// <param name="initRequest">IRestRequest initialization.</param>
        /// <param name="apiMethodName">Strong-typed REST API method name, for tracing.</param>
        public T Put<T>(string url, object body, Action<IRestRequest> initRequest = null, [CallerMemberName] string apiMethodName = null) =>
            Execute<T>(new RestRequest(url, Method.PUT, DataFormat.Json), body, initRequest, apiMethodName);
    }
}
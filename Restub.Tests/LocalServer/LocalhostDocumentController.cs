using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace Restub.Tests.LocalServer
{
    /// <summary>
    /// EmbedIO web api controller for localhost demo service.
    /// </summary>
    public class LocalhostDocumentController : WebApiController
    {
        /// <summary>
        /// Generates a deterministic garbage string.
        /// </summary>
        /// <param name="order">Determines the length.</param>
        public static string Garbage(int order) =>
            string.Join(string.Empty, Enumerable.Repeat("1", order)
                .Select((s, i) => $"{s}{new string((char)('0' + i % 10), i)}"));

        private static ConcurrentDictionary<int, string> Documents { get; } =
            PopulateDocuments();

        private static ConcurrentDictionary<int, string> PopulateDocuments()
        {
            var result = new ConcurrentDictionary<int, string>();
            result[LastID] = "One";
            result[LastID] = "Two";
            result[LastID] = "Three";
            result[LastID] = Garbage(15);
            return result;
        }

        private static int lastId;

        private static int LastID => Interlocked.Increment(ref lastId);

        [Route(HttpVerbs.Get, "/docs")]
        public Task<ICollection<string>> GetAllDocuments() =>
            Task.FromResult(Documents.Values);

        [Route(HttpVerbs.Get, "/docs/{id}")]
        public async Task GetDocument(int id)
        {
            if (!Documents.TryGetValue(id, out var doc))
            {
                throw new HttpException(HttpStatusCode.NotFound);
            }

            using (var writer = HttpContext.OpenResponseText())
            {
                await writer.WriteAsync(doc);
            }
        }

        [Route(HttpVerbs.Get, "/docs/{id}/bytes")]
        public async Task GetDocumentBytes(int id)
        {
            if (!Documents.TryGetValue(id, out var doc))
            {
                throw new HttpException(HttpStatusCode.NotFound);
            }

            Response.ContentType = "application/octet-stream";

            using (var writer = HttpContext.OpenResponseStream())
            {
                var bytes = Encoding.UTF8.GetBytes(doc);
                await writer.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        [Route(HttpVerbs.Put, "/docs/{id}")]
        public async Task UpdateDocument(int id)
        {
            var doc = await HttpContext.GetRequestBodyAsStringAsync();
            Documents[id] = doc;

            using (var writer = HttpContext.OpenResponseText())
            {
                await writer.WriteAsync(doc);
            }
        }

        [Route(HttpVerbs.Post, "/docs/add")]
        public async Task<int> AddDocument()
        {
            var doc = await HttpContext.GetRequestBodyAsStringAsync();
            var id = LastID;
            Documents[id] = doc;
            return id;
        }

        [Route(HttpVerbs.Delete, "/docs/{id}")]
        public async Task DeleteDocument(int id)
        {
            if (!Documents.TryRemove(id, out var doc))
            {
                throw new HttpException(HttpStatusCode.NotFound);
            }

            using (var writer = HttpContext.OpenResponseText())
            {
                await writer.WriteAsync(doc);
            }
        }

        [Route(HttpVerbs.Get, "/docs/setcookie/{name}/{value}")]
        public Task SetCookie(string name, string value)
        {
            var cookie = new Cookie(name, value);
            Response.Cookies.Add(cookie);

            return HttpContext.SendStringAsync($"{name} = {value}", MimeType.PlainText, WebServer.DefaultEncoding);
        }
    }
}

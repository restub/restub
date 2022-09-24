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
        private static ConcurrentDictionary<int, string> Documents { get; } =
            PopulateDocuments();

        private static ConcurrentDictionary<int, string> PopulateDocuments()
        {
            var result = new ConcurrentDictionary<int, string>();
            result[LastID] = "One";
            result[LastID] = "Two";
            result[LastID] = "Three";
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
    }
}

using System;
using System.Threading.Tasks;

namespace Restub.Tests.LocalServer
{
    /// <remarks>
    /// Local REST API server sample client, string methods, asynchronous.
    /// </remarks>
    public partial class LocalhostClient
    {
        public Task<string[]> GetAllDocumentsAsync() =>
            GetAsync<string[]>("/api/docs");

        public Task<string> GetDocumentAsync(int id) =>
            GetAsync<string>("/api/docs/{id}", r => r.AddUrlSegment("id", id));

        public Task<string> UpdateDocumentAsync(int id, string doc) =>
            PutAsync<string>("/api/docs/{id}", doc, r => r.AddUrlSegment("id", id));

        public async Task<int> AddDocumentAsync(string doc) =>
            Convert.ToInt32(await PostAsync<string>("/api/docs/add", doc));

        public Task<string> DeleteDocumentAsync(int id) =>
            DeleteAsync<string>("/api/docs/{id}", null, r => r.AddUrlSegment("id", id));
    }
}

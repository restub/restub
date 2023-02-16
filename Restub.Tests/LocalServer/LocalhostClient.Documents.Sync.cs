using System;

namespace Restub.Tests.LocalServer
{
    /// <remarks>
    /// Local REST API server sample client, string methods, synchronous.
    /// </remarks>
    public partial class LocalhostClient
    {
        public string[] GetAllDocuments() =>
            Get<string[]>("/api/docs");

        public string GetDocument(int id) =>
            Get<string>("/api/docs/{id}", r => r.AddUrlSegment("id", id));

        public byte[] GetDocumentBytes(int id) =>
            Get<byte[]>("/api/docs/{id}/bytes", r => r.AddUrlSegment("id", id));

        public string UpdateDocument(int id, string doc) =>
            Put<string>("/api/docs/{id}", doc, r => r.AddUrlSegment("id", id));

        public int AddDocument(string doc) =>
            Convert.ToInt32(Post<string>("/api/docs/add", doc));

        public string DeleteDocument(int id) =>
            Delete<string>("/api/docs/{id}", null, r => r.AddUrlSegment("id", id));

        public string SetCookie(string name, string value) =>
            Get<string>("/api/docs/setcookie/{name}/{value}", r => r
                .AddUrlSegment("name", name)
                .AddUrlSegment("value", value));
    }
}

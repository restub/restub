namespace Restub.Tests.LocalServer
{
    /// <remarks>
    /// Local REST API server sample client, object methods, synchronous.
    /// </remarks>
    public partial class LocalhostClient
    {
        public LocalhostPerson[] GetAllPeople() =>
            Get<LocalhostPerson[]>("/api/people");

        public LocalhostPerson GetPerson(int id) =>
            Get<LocalhostPerson>("/api/people/{id}", r => r.AddUrlSegment("id", id));

        public LocalhostPerson UpdatePerson(int id, string name, decimal size) =>
            Put<LocalhostPerson>("/api/people/{id}", new LocalhostPerson
            {
                Name = name,
                Size = size,
            },
            r => r.AddUrlSegment("id", id));

        public LocalhostPerson PatchPerson(int id, string name = null, decimal size = 0) =>
            Patch<LocalhostPerson>("/api/people/{id}", new LocalhostPerson
            {
                Name = name,
                Size = size,
            },
            r => r.AddUrlSegment("id", id));

        public LocalhostPerson AddPerson(string name, decimal size) =>
            Post<LocalhostPerson>("/api/people/add", new LocalhostPerson
            {
                Name = name,
                Size = size,
            });

        public LocalhostPerson DeletePerson(int id) =>
            Delete<LocalhostPerson>("/api/people/{id}", null, r => r.AddUrlSegment("id", id));
    }
}

using System.Threading.Tasks;

namespace Restub.Tests.LocalServer
{
    /// <remarks>
    /// Local REST API server sample client, object methods, asynchronous.
    /// </remarks>
    public partial class LocalhostClient
    {
        public Task<LocalhostPerson[]> GetAllPeopleAsync() =>
            GetAsync<LocalhostPerson[]>("/api/people");

        public Task<LocalhostPerson> GetPersonAsync(int id) =>
            GetAsync<LocalhostPerson>("/api/people/{id}", r => r.AddUrlSegment("id", id));

        public Task<LocalhostPerson> UpdatePersonAsync(int id, string name, decimal size) =>
            PutAsync<LocalhostPerson>("/api/people/{id}", new LocalhostPerson
            {
                Name = name,
                Size = size,
            },
            r => r.AddUrlSegment("id", id));

        public Task<LocalhostPerson> PatchPersonAsync(int id, string name = null, decimal size = 0) =>
            PatchAsync<LocalhostPerson>("/api/people/{id}", new LocalhostPerson
            {
                Name = name,
                Size = size,
            },
            r => r.AddUrlSegment("id", id));

        public Task<LocalhostPerson> AddPersonAsync(string name, decimal size) =>
            PostAsync<LocalhostPerson>("/api/people/add", new LocalhostPerson
            {
                Name = name,
                Size = size,
            });

        public Task<LocalhostPerson> DeletePersonAsync(int id) =>
            DeleteAsync<LocalhostPerson>("/api/people/{id}", null, r => r.AddUrlSegment("id", id));
    }
}

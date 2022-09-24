using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
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
    public class LocalhostPersonController : WebApiController
    {
        private static ConcurrentDictionary<int, LocalhostPerson> People { get; } =
            PopulatePeople();

        private static ConcurrentDictionary<int, LocalhostPerson> PopulatePeople()
        {
            var people = new[]
            {
                new LocalhostPerson { ID = LastID, Name = "Alice", Size = 123, },
                new LocalhostPerson { ID = LastID, Name = "Bob", Size = 321, },
                new LocalhostPerson { ID = LastID, Name = "Carl", Size = 222, },
            };

            var result = new ConcurrentDictionary<int, LocalhostPerson>();
            foreach (var p in people)
            {
                result[p.ID] = p;
            }

            return result;
        }

        private static int lastId;

        private static int LastID => Interlocked.Increment(ref lastId);

        [Route(HttpVerbs.Get, "/people")]
        public Task<ICollection<LocalhostPerson>> GetAllPeople() =>
            Task.FromResult(People.Values);

        [Route(HttpVerbs.Get, "/people/{id}")]
        public Task<LocalhostPerson> GetPerson(int id) =>
            Task.FromResult(People.TryGetValue(id, out var person) ? person :
                throw new HttpException(HttpStatusCode.NotFound));

        [Route(HttpVerbs.Put, "/people/{id}")]
        public Task<LocalhostPerson> UpdatePerson(int id, [JsonData] LocalhostPerson person)
        {
            person.ID = id;
            People[person.ID] = person;
            return Task.FromResult(person);
        }

        [Route(HttpVerbs.Patch, "/people/{id}")]
        public Task<LocalhostPerson> PatchPerson(int id, [JsonData] LocalhostPerson person)
        {
            var existingPerson = People.TryGetValue(id, out var p) ? p :
                throw new HttpException(HttpStatusCode.NotFound);

            existingPerson.Name = person.Name ?? existingPerson.Name;
            if (person.Size != 0)
            {
                existingPerson.Size = person.Size;
            }

            return Task.FromResult(existingPerson);
        }

        [Route(HttpVerbs.Post, "/people/add")]
        public Task<LocalhostPerson> AddPerson([JsonData] LocalhostPerson person)
        {
            person.ID = LastID;
            People[person.ID] = person;
            return Task.FromResult(person);
        }

        [Route(HttpVerbs.Delete, "/people/{id}")]
        public Task<LocalhostPerson> DeletePerson(int id) =>
            Task.FromResult(People.TryRemove(id, out var result) ? result :
                throw new HttpException(HttpStatusCode.NotFound));
    }
}

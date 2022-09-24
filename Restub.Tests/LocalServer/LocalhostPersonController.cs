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
    public class LocalhostPersonController : WebApiController
    {
        private static ConcurrentDictionary<int, LocalhostPerson> People { get; } =
            PopulatePeople();

        private static ConcurrentDictionary<int, LocalhostPerson> PopulatePeople()
        {
            var result = new ConcurrentDictionary<int, LocalhostPerson>();
            result[1] = new LocalhostPerson { ID = LastID, Name = "Alice", Size = 123, };
            result[2] = new LocalhostPerson { ID = LastID, Name = "Bob", Size = 321, };
            result[3] = new LocalhostPerson { ID = LastID, Name = "Carl", Size = 222, };
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

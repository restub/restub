using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restub.Tests.LocalServer
{
    public class LocalhostClient : RestubClient
    {
        public LocalhostClient(int port) : base("http://127.0.0.1:" + port)
        {
        }

        public string Hello() => Get("/");

        public LocalhostDummy PostDummy(string name, int size) =>
            Post<LocalhostDummy>("/dummy", new LocalhostDummy { Name = name, Size = size });
    }
}

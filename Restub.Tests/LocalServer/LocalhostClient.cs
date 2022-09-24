using System.Threading.Tasks;

namespace Restub.Tests.LocalServer
{
    /// <summary>
    /// Local REST API server sample client.
    /// </summary>
    public partial class LocalhostClient : RestubClient
    {
        public LocalhostClient(int port) : base("http://127.0.0.1:" + port)
        {
        }

        public string Hello() => Get<string>("/");

        public Task<string> HelloAsync() => GetAsync<string>("/");
    }
}

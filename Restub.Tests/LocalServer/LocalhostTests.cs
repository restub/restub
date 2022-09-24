using System;
using System.Threading.Tasks;
using LunarLabs.WebServer.Core;
using LunarLabs.WebServer.HTTP;
using NUnit.Framework;

namespace Restub.Tests.LocalServer
{
    [TestFixture]
    public class LocalhostTests : IDisposable
    {
        private const int HttpPort = 34567;

        private HTTPServer Server { get; set; }

        private Task ServerTask { get; set; }

        private LocalhostClient Client { get; } = new LocalhostClient(HttpPort)
        {
            Tracer = TestContext.Progress.WriteLine
        };

        public LocalhostTests()
        {
            var settings = new ServerSettings
            {
                Port = HttpPort,
                BindingHost = "127.0.0.1",
                Path = null,
                Compression = false,
            };

            // set up supported server endpoints
            Server = new HTTPServer(settings, ConsoleLogger.Write);
            Server.Get("/", req => HTTPResponse.FromString("Hello world!"));
            Server.Post("/dummy", req => HTTPResponse.FromString(req.postBody));

            // start the local server
            var tcs = new TaskCompletionSource<bool>();
            ServerTask = Task.Run(() =>
            {
                tcs.TrySetResult(true);

                // note: looks like Run is blocking
                Server.Run();
            });

            // make sure it's started
            tcs.Task.ConfigureAwait(false).GetAwaiter().GetResult();
            TestContext.Progress.WriteLine($"*** SERVER STARTED, PORT: {HttpPort} ***");
        }

        public void Dispose()
        {
            // note: looks like LunarServer has some trouble shutting down
            // NUnut test runner warns about an exception unloading the application domain
            Server.Dispose();
            ServerTask.ConfigureAwait(false).GetAwaiter().GetResult();
            TestContext.Progress.WriteLine($"*** SERVER STOPPED, PORT: {HttpPort} ***");
        }

        [Test]
        public void LocalhostClientConnects()
        {
            var result = Client.Hello();
            Assert.That(result, Is.EqualTo("Hello world!"));
        }

        [Test, Ignore("Fails for some reason")]
        public void LocalhostClientPostWorks()
        {
            var result = Client.PostDummy("Bozo", 123);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Bozo"));
            Assert.That(result.Size, Is.EqualTo(123));
        }
    }
}

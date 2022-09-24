using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.WebApi;
using NUnit.Framework;

namespace Restub.Tests.LocalServer
{
    [TestFixture]
    public class LocalhostTests : IDisposable
    {
        private const int HttpPort = 34567;

        private WebServer Server { get; set; }

        private Task ServerTask { get; set; }

        private CancellationTokenSource ServerCancel { get; set; } = new CancellationTokenSource();

        private LocalhostClient Client { get; } = new LocalhostClient(HttpPort)
        {
            Tracer = TestContext.Progress.WriteLine
        };

        public LocalhostTests()
        {
            // start the local server
            Server = CreateWebServer();
            ServerTask = Server.RunAsync(ServerCancel.Token);

            // make sure it's started
            TestContext.Progress.WriteLine($"*** SERVER STARTED, PORT: {HttpPort} ***");
        }

        private WebServer CreateWebServer() =>
            new WebServer(o => o
                .WithUrlPrefix("http://127.0.0.1:" + HttpPort)
                .WithMode(HttpListenerMode.EmbedIO))
                .WithWebApi("/api", m => m.WithController<LocalhostPersonController>())
                .WithModule(new ActionModule("/", HttpVerbs.Any, c => c
                    .SendStringAsync("Hello world!", "text/plain", Encoding.ASCII)));

        public void Dispose()
        {
            // make sure that server is stopped and the port is freed
            ServerCancel.Cancel();
            ServerTask.ConfigureAwait(false).GetAwaiter().GetResult();
            TestContext.Progress.WriteLine($"*** SERVER STOPPED, PORT: {HttpPort} ***");
        }

        [Test]
        public void LocalhostClientConnects()
        {
            var result = Client.Hello();
            Assert.That(result, Is.EqualTo("Hello world!"));

            Assert.That(() => Client.Get<string>("/api/quux"),
                Throws.TypeOf<RestubException>()
                    .With.Property(nameof(RestubException.StatusCode))
                        .EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void LocalhostGetAllPeople()
        {
            var result = Client.GetAllPeople();
            Assert.That(result, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void LocalhostGetPerson()
        {
            var result = Client.GetPerson(2);
            Assert.That(result, Is.Not.Null.Or.Empty);
            Assert.That(result.ID, Is.EqualTo(2));
            Assert.That(result.Name, Is.EqualTo("Bob"));
            Assert.That(result.Size, Is.Not.EqualTo(0));
        }

        [Test]
        public void LocalhostClientAddPerson()
        {
            var result = Client.AddPerson("Bozo", 123);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.Not.EqualTo(0));
            Assert.That(result.Name, Is.EqualTo("Bozo"));
            Assert.That(result.Size, Is.EqualTo(123));
        }

        [Test]
        public void LocalhostClientUpdatePerson()
        {
            // all properties are required
            var result = Client.UpdatePerson(12, "Foobar", 333);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(12));
            Assert.That(result.Name, Is.EqualTo("Foobar"));
            Assert.That(result.Size, Is.EqualTo(333m));
        }

        [Test]
        public void LocalhostClientPatchPerson()
        {
            // only some properties can be specified
            var result = Client.PatchPerson(1, "Alifie");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Alifie"));
            Assert.That(result.Size, Is.EqualTo(123m));

            result = Client.PatchPerson(2, null, 333m);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(2));
            Assert.That(result.Name, Is.EqualTo("Bob"));
            Assert.That(result.Size, Is.EqualTo(333m));
        }

        [Test]
        public void LocalhostClientDeletePerson()
        {
            var result = Client.DeletePerson(3);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(3));
            Assert.That(result.Name, Is.EqualTo("Carl"));
            Assert.That(result.Size, Is.EqualTo(222m));

            Assert.That(() => Client.DeletePerson(111),
                Throws.TypeOf<RestubException>()
                    .With.Property(nameof(RestubException.StatusCode))
                        .EqualTo(HttpStatusCode.NotFound));
        }
    }
}

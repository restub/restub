using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.Authentication;
using EmbedIO.WebApi;
using NUnit.Framework;
using RestSharp;

namespace Restub.Tests.LocalServer
{
    /// <summary>
    /// Sends REST requests to the local web server.
    /// </summary>
    [TestFixture]
    public sealed class LocalhostTests : IDisposable
    {
        private const int HttpPort = 34567;

        private const string UserName = "scott";

        private const string Password = "tiger";

        private WebServer Server { get; set; }

        private Task ServerTask { get; set; }

        private CancellationTokenSource ServerCancel { get; set; } = new CancellationTokenSource();

        private LocalhostClient Client { get; } = new LocalhostClient(HttpPort, UserName, Password)
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

        private static WebServer CreateWebServer() =>
            new WebServer(o => o
                .WithUrlPrefix("http://127.0.0.1:" + HttpPort)
                .WithMode(HttpListenerMode.EmbedIO))
                .WithModule(new BasicAuthenticationModule("/")
                    .WithAccount(UserName, Password))
                .WithWebApi("/api", m => m
                    .WithController<LocalhostPersonController>()
                    .WithController<LocalhostDocumentController>())
                .WithModule(new ActionModule("/", HttpVerbs.Any, c => c
                    .SendStringAsync("Hello world!", "text/plain", Encoding.ASCII)));

        public void Dispose()
        {
            // make sure that server is stopped and the port is freed
            ServerCancel.Cancel();
            ServerTask.ConfigureAwait(false).GetAwaiter().GetResult();
            TestContext.Progress.WriteLine($"*** SERVER STOPPED, PORT: {HttpPort} ***");
        }

        // People — Synchronous

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
            Assert.That(result, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void LocalhostGetPerson()
        {
            var result = Client.GetPerson(2);
            Assert.That(result, Is.Not.Null);
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
            Client.UpdatePerson(3, "Keith", 234);

            var result = Client.DeletePerson(3);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(3));
            Assert.That(result.Name, Is.EqualTo("Keith"));
            Assert.That(result.Size, Is.EqualTo(234m));

            Assert.That(() => Client.DeletePerson(111),
                Throws.TypeOf<RestubException>()
                    .With.Property(nameof(RestubException.StatusCode))
                        .EqualTo(HttpStatusCode.NotFound));
        }

        // People — Asynchronous

        [Test]
        public async Task LocalhostClientConnectsAsync()
        {
            var result = await Client.HelloAsync();
            Assert.That(result, Is.EqualTo("Hello world!"));

            Assert.That(async () => await Client.GetAsync<string>("/api/quux"),
                Throws.TypeOf<RestubException>()
                    .With.Property(nameof(RestubException.StatusCode))
                        .EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task LocalhostGetAllPeopleAsync()
        {
            var result = await Client.GetAllPeopleAsync();
            Assert.That(result, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task LocalhostGetPersonAsync()
        {
            var result = await Client.GetPersonAsync(2);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(2));
            Assert.That(result.Name, Is.EqualTo("Bob"));
            Assert.That(result.Size, Is.Not.EqualTo(0));
        }

        [Test]
        public async Task LocalhostClientAddPersonAsync()
        {
            var result = await Client.AddPersonAsync("Bozo", 123);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.Not.EqualTo(0));
            Assert.That(result.Name, Is.EqualTo("Bozo"));
            Assert.That(result.Size, Is.EqualTo(123));
        }

        [Test]
        public async Task LocalhostClientUpdatePersonAsync()
        {
            // all properties are required
            var result = await Client.UpdatePersonAsync(12, "Foobar", 333);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(12));
            Assert.That(result.Name, Is.EqualTo("Foobar"));
            Assert.That(result.Size, Is.EqualTo(333m));
        }

        [Test]
        public async Task LocalhostClientPatchPersonAsync()
        {
            // only some properties can be specified
            var result = await Client.PatchPersonAsync(1, "Porpoise");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Porpoise"));
            Assert.That(result.Size, Is.EqualTo(123m));

            result = await Client.PatchPersonAsync(2, null, 111m);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(2));
            Assert.That(result.Name, Is.EqualTo("Bob"));
            Assert.That(result.Size, Is.EqualTo(111m));
        }

        [Test]
        public async Task LocalhostClientDeletePersonAsync()
        {
            await Client.UpdatePersonAsync(33, "Palmer", 432m);

            var result = await Client.DeletePersonAsync(33);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(33));
            Assert.That(result.Name, Is.EqualTo("Palmer"));
            Assert.That(result.Size, Is.EqualTo(432m));

            Assert.That(async () => await Client.DeletePersonAsync(111),
                Throws.TypeOf<RestubException>()
                    .With.Property(nameof(RestubException.StatusCode))
                        .EqualTo(HttpStatusCode.NotFound));
        }

        // Documents — synchronous

        [Test]
        public void LocalhostGetAllDocuments()
        {
            var result = Client.GetAllDocuments();
            Assert.That(result, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void LocalhostGetDocument()
        {
            var result = Client.GetDocument(2);
            Assert.That(result, Is.EqualTo("Two"));
        }

        [Test]
        public void LocalhostGetDocumentBytes()
        {
            var result = Client.GetDocumentBytes(4);
            var garbage = LocalhostDocumentController.Garbage(15);
            Assert.That(result, Is.EqualTo(Encoding.UTF8.GetBytes(garbage)));
        }

        [Test]
        public void LocalhostClientAddDocument()
        {
            var result = Client.AddDocument("Bozo");
            Assert.That(result, Is.Not.EqualTo(0));
        }

        [Test]
        public void LocalhostClientUpdateDocument()
        {
            var result = Client.UpdateDocument(12, "Foobar");
            Assert.That(result, Is.EqualTo("Foobar"));
        }

        [Test]
        public void LocalhostClientDeleteDocument()
        {
            Client.UpdateDocument(3, "Three");

            var result = Client.DeleteDocument(3);
            Assert.That(result, Is.EqualTo("Three"));

            Assert.That(() => Client.DeleteDocument(111),
                Throws.TypeOf<RestubException>()
                    .With.Property(nameof(RestubException.StatusCode))
                        .EqualTo(HttpStatusCode.NotFound));
        }

        // Documents — asynchronous

        [Test]
        public async Task LocalhostGetAllDocumentsAsync()
        {
            var result = await Client.GetAllDocumentsAsync();
            Assert.That(result, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task LocalhostGetDocumentAsync()
        {
            var result = await Client.GetDocumentAsync(2);
            Assert.That(result, Is.EqualTo("Two"));
        }

        [Test]
        public async Task LocalhostGetDocumentBytesAsync()
        {
            var result = await Client.GetDocumentBytesAsync(4);
            var garbage = LocalhostDocumentController.Garbage(15);
            Assert.That(result, Is.EqualTo(Encoding.UTF8.GetBytes(garbage)));
        }

        [Test]
        public async Task LocalhostClientAddDocumentAsync()
        {
            var result = await Client.AddDocumentAsync("Bozo");
            Assert.That(result, Is.Not.EqualTo(0));
        }

        [Test]
        public async Task LocalhostClientUpdateDocumentAsync()
        {
            var result = await Client.UpdateDocumentAsync(12, "Foobar");
            Assert.That(result, Is.EqualTo("Foobar"));
        }

        [Test]
        public async Task LocalhostClientDeleteDocumentAsync()
        {
            await Client.UpdateDocumentAsync(3, "Three");

            var result = await Client.DeleteDocumentAsync(3);
            Assert.That(result, Is.EqualTo("Three"));

            Assert.That(async () => await Client.DeleteDocumentAsync(111),
                Throws.TypeOf<RestubException>()
                    .With.Property(nameof(RestubException.StatusCode))
                        .EqualTo(HttpStatusCode.NotFound));
        }

        // event handlers and cookies

        private async Task TestBeforeAndAfterExecute(Func<Task> executeTest)
        {
            var beforeExecuteCalled = false;
            void beforeExecute(object sender, IRestRequest request) =>
                beforeExecuteCalled = true;

            var afterExecuteCalled = false;
            void afterExecute(object sender, IRestResponse response) =>
                afterExecuteCalled = true;

            try
            {
                Client.BeforeExecuteRequest += beforeExecute;
                Client.AfterExecuteRequest += afterExecute;

                Assert.That(beforeExecuteCalled, Is.False);
                Assert.That(afterExecuteCalled, Is.False);

                await executeTest();

                Assert.That(beforeExecuteCalled, Is.True);
                Assert.That(afterExecuteCalled, Is.True);
            }
            finally 
            {
                Client.BeforeExecuteRequest -= beforeExecute;
                Client.AfterExecuteRequest -= afterExecute;
            }
        }

        [Test]
        public async Task BeforeAndAfterExecuteTests()
        {
            // Execute<T> and ExecuteAsync<T>
            await TestBeforeAndAfterExecute(() => Client.GetAllDocumentsAsync());
            await TestBeforeAndAfterExecute(() =>
            {
                Client.GetAllDocuments();
                return Task.CompletedTask;
            });

            // Execute and ExecuteAsync
            await TestBeforeAndAfterExecute(() => Client.GetDocumentBytesAsync(2));
            await TestBeforeAndAfterExecute(() =>
            {
                Client.GetDocumentBytes(2);
                return Task.CompletedTask;
            });
        }

        private async Task TestCookies(string name, string value, Func<Task> executeTest)
        {
            void afterExecute(object sender, IRestResponse response)
            {
                var index = response.Cookies.ToDictionary(c => c.Name, c => c.Value);
                Assert.That(index[name], Is.EqualTo(value));
            }

            Client.AfterExecuteRequest += afterExecute;
            try
            {
                await executeTest();
            }
            finally
            {
                Client.AfterExecuteRequest -= afterExecute;
            }
        }

        [Test]
        public async Task TestCookiesSyncAndAsync()
        {
            await TestCookies("Hello", "World", () =>
            {
                var result = Client.SetCookie("Hello", "World");
                Assert.That(result, Is.EqualTo("Hello = World"));
                return Task.CompletedTask;
            });

            await TestCookies("Goodbye", "There", async () =>
            {
                var result = await Client.SetCookieAsync("Goodbye", "There");
                Assert.That(result, Is.EqualTo("Goodbye = There"));
            });

#if !NET462
            // all cookies should be now set
            var cookies = Client.Client.CookieContainer
                .GetAllCookies().OfType<Cookie>()
                .ToDictionary(c => c.Name, c => c.Value);

            Assert.That(cookies["Hello"], Is.EqualTo("World"));
            Assert.That(cookies["Goodbye"], Is.EqualTo("There"));
#endif
        }
    }
}

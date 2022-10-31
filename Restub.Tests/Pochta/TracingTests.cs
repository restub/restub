using System.Text;
using NUnit.Framework;
using Restub.Tests.LocalServer;

namespace Restub.Tests.Pochta
{
    [TestFixture]
    public class TracingTests
    {
        private string Fmt(byte[] bytes) => RestubClient.FormatBytes(bytes);

        [Test]
        public void FormatBytesSkipsEmptyData()
        {
            Assert.That(Fmt(null), Is.EqualTo(string.Empty));
            Assert.That(Fmt(new byte[0]), Is.EqualTo(string.Empty));
        }

        [Test]
        public void FormatBytesFormatsNonEmptyData()
        {
            var testGarbage = LocalhostDocumentController.Garbage(20);
            var testBytes = Encoding.UTF8.GetBytes(testGarbage);

            Assert.That(Fmt(testBytes), Is.EqualTo("210 bytes: {\r\n" +
            "  31 31 31 31 32 32 31 33 33 33 31 34 34 34 34 31\r\n" +
            "  35 35 35 35 35 31 36 36 36 36 36 36 31 37 37 37\r\n" +
            "  37 37 37 37 31 38 38 38 38 38 38 38 38 31 39 39\r\n" +
            "  39 39 39 39 39 39 39 31 30 30 30 30 30 30 30 30\r\n" +
            "  30 30 31 31 31 31 31 31 31 31 31 31 31 31 31 32\r\n" +
            "  32 32 32 32 32 32 32 32 32 32 32 31 33 33 33 33\r\n" +
            "  33 33 33 33 33 33 33 33 33 31 34 34 34 34 34 34\r\n" +
            "  34 34 34 34 34 34 34 34 31 35 35 35 35 35 35 35\r\n" +
            "  35 35 35 35 35 35 35 35 31 36 36 36 36 36 36 36\r\n" +
            "  36 36 36 36 36 36 36 36 36 31 37 37 37 37 37 37\r\n" +
            "  37 37 37 37 37 37 37 37 37 37 37 31 38 38 38 38\r\n" +
            "  38 38 38 38 38 38 38 38 38 38 38 38 38 38 31 39\r\n" +
            "  39 39 39 39 39 39 39 39 39 39 39 39 39 39 39 39\r\n" +
            "  39 39\r\n" +
            "}\r\n"));

            testGarbage = testGarbage + testGarbage;
            testBytes = Encoding.UTF8.GetBytes(testGarbage);

            Assert.That(Fmt(testBytes), Is.EqualTo("420 bytes: {\r\n" +
            "  31 31 31 31 32 32 31 33 33 33 31 34 34 34 34 31\r\n" +
            "  35 35 35 35 35 31 36 36 36 36 36 36 31 37 37 37\r\n" +
            "  37 37 37 37 31 38 38 38 38 38 38 38 38 31 39 39\r\n" +
            "  39 39 39 39 39 39 39 31 30 30 30 30 30 30 30 30\r\n" +
            "  30 30 31 31 31 31 31 31 31 31 31 31 31 31 31 32\r\n" +
            "  32 32 32 32 32 32 32 32 32 32 32 31 33 33 33 33\r\n" +
            "  33 33 33 33 33 33 33 33 33 31 34 34 34 34 34 34\r\n" +
            "  34 34 34 34 34 34 34 34 31 35 35 35 35 35 35 35\r\n" +
            "  35 35 35 35 35 35 35 35 31 36 36 36 36 36 36 36\r\n" +
            "  36 36 36 36 36 36 36 36 36 31 37 37 37 37 37 37\r\n" +
            "  37 37 37 37 37 37 37 37 37 37 37 31 38 38 38 38\r\n" +
            "  38 38 38 38 38 38 38 38 38 38 38 38 38 38 31 39\r\n" +
            "  39 39 39 39 39 39 39 39 39 39 39 39 39 39 39 39\r\n" +
            "  39 39 31 31 31 31 32 32 31 33 33 33 31 34 34 34\r\n" +
            "  34 31 35 35 35 35 35 31 36 36 36 36 36 36 31 37\r\n" +
            "  37 37 37 37 37 37 31 38 38 38 38 38 38 38 38 31\r\n" +
            "  ...\r\n" +
            "}\r\n"));
        }
    }
}

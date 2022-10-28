using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restub.Toolbox;
using NUnit.Framework;

namespace Restub.Tests
{
    [TestFixture]
    public class JsonFormatterTests
    {
        string F(string j) => JsonFormatter.FormatJson(j);

        [Test]
        public void EmptyStringsAreIgnored()
        {
            Assert.That(F(null), Is.EqualTo(string.Empty));
            Assert.That(F(string.Empty), Is.EqualTo(string.Empty));
            Assert.That(F("   "), Is.EqualTo(string.Empty));
        }

        [Test]
        public void ScalarValuesAreFormatted()
        {
            Assert.That(F("1"), Is.EqualTo("1"));
            Assert.That(F("  1 "), Is.EqualTo("1"));
            Assert.That(F(" \"s \""), Is.EqualTo("\"s \""));
            Assert.That(F(" \"s\\t\\r \""), Is.EqualTo("\"s\\t\\r \""));
        }

        [Test]
        public void ObjectValuesAreFormatted()
        {
            Assert.That(F("{}"), Is.EqualTo("{\r\n  \r\n}"));
            Assert.That(F("{\"i\":1,\"s\":\"321\"}"), Is.EqualTo("{\r\n  \"i\": 1,\r\n  \"s\": \"321\"\r\n}"));
            Assert.That(F("{\"o\":{\"t\":null},\"i\":1,\"s\":\"321\"}"), Is.EqualTo("{\r\n  \"o\": {\r\n    \"t\": null\r\n  },\r\n  \"i\": 1,\r\n  \"s\": \"321\"\r\n}"));
        }

        [Test]
        public void ArrayValuesAreFormatted()
        {
            Assert.That(F("[]"), Is.EqualTo("[\r\n  \r\n]"));
            Assert.That(F("[\"i\",1,\"s\",321]"), Is.EqualTo("[\r\n  \"i\",\r\n  1,\r\n  \"s\",\r\n  321\r\n]"));
            Assert.That(F("{\"o\":{\"t\":[]},\"i\":[1,\"s\",\"321\"]}"), Is.EqualTo("{\r\n  \"o\": {\r\n    \"t\": [\r\n      \r\n    ]\r\n  },\r\n  \"i\": [\r\n    1,\r\n    \"s\",\r\n    \"321\"\r\n  ]\r\n}"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;
using Pair = System.Collections.DictionaryEntry;

namespace Restub.Tests
{
    [TestFixture]
    public class TestBase
    {
        static TestBase()
        {
            var vars = Environment.GetEnvironmentVariables().OfType<Pair>();
            foreach (var var in vars.OrderBy(v => v.Key))
            {
                TestContext.Progress.WriteLine($"{var.Key} = {var.Value}");
            }

            if (string.IsNullOrEmpty(Env("TEST_ENVIRONMENT_INITIALIZED")))
            {
                SetEnvironmentVariables(@".temp\vars.json");
            }
        }

        private static void SetEnvironmentVariables(string fileName)
        {
            // go to the project's root directory
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            while (dir.Length > 0 && !dir.EndsWith(nameof(Restub), StringComparison.OrdinalIgnoreCase))
            {
                dir = Path.GetDirectoryName(dir);
            }

            // read the variables file
            var pathName = Path.Combine(dir, fileName);
            var contents = File.ReadAllText(pathName);
            var variables = DeserializeDictionary(contents);

            // set environment variables
            foreach (var var in variables)
            {
                Environment.SetEnvironmentVariable(var.Key, var.Value);
            }
        }

        protected static Dictionary<string, string> DeserializeDictionary(string json) =>
            JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        protected static string Env(string name) =>
            Environment.GetEnvironmentVariable(name);

        [Test]
        public void EnvironmentVariablesAreSet() =>
            Assert.That(Env("TEST_ENVIRONMENT_INITIALIZED"), Is.EqualTo("true"));
    }
}

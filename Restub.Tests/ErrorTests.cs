using System;
using System.Collections.Generic;
using System.Linq;
using Restub.DataContracts;
using NUnit.Framework;

namespace Restub.Tests
{
    [TestFixture]
    public class ErrorTests
    {
        [Test]
        public void ErrorResponseGetsConverterToAStringMessage()
        {
            Assert.That(RestubClient.GetErrorMessage(null), Is.EqualTo(string.Empty));
            Assert.That(RestubClient.GetErrorMessage(new ErrorResponse()), Is.EqualTo(string.Empty));

            var msg = RestubClient.GetErrorMessage(new ErrorResponse
            {
                Errors = new List<Error>
                {
                    new Error { Message = "Hello" },
                    new Error { Message = "Cruel" },
                    new Error { Message = "World" },
                }
            });

            Assert.That(msg, Is.EqualTo("Hello. Cruel. World"));
        }

        [Test]
        public void EmptyResponseGetErrorsDoesntThrow()
        {
            // all DTO types implementing IHasErrors interface
            var errorEnabledResponseTypes =
                from t in typeof(IHasErrors).Assembly.GetTypes()
                where t.GetInterfaces().Contains(typeof(IHasErrors))
                select t;

            var responseTypes = errorEnabledResponseTypes.ToArray();
            Assert.That(responseTypes, Is.Not.Null.Or.Empty);
            Assert.That(responseTypes.Length, Is.GreaterThan(0));

            foreach (var t in responseTypes)
            {
                var emptyResponse = Activator.CreateInstance(t) as IHasErrors;
                Assert.That(emptyResponse, Is.Not.Null);
                Assert.That(emptyResponse.GetErrors(), Is.Not.Null);
                Assert.That(emptyResponse.GetErrors().Any(), Is.False);
            }
        }
    }
}
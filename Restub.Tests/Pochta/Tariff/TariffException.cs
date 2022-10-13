using System;
using System.Net;

namespace Restub.Tests.Pochta.Tariff
{
    /// <summary>
    /// Pochta.ru tariffication API sample client exception.
    /// https://tariff.pochta.ru/
    /// </summary>
    public class TariffException : RestubException
    {
        public TariffException(HttpStatusCode code, string message, Exception innerException = null)
            : base(code, message, innerException)
        {
        }
    }
}

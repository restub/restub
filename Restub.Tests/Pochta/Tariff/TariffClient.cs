namespace Restub.Tests.Pochta.Tariff
{
    /// <summary>
    /// Pochta.ru tariffication API sample client, requires no authentication.
    /// https://tariff.pochta.ru/
    /// </summary>
    public class TariffClient : RestubClient
    {
        public TariffClient(string baseUrl) : base(baseUrl)
        {
        }

        protected override Authenticator CreateAuthenticator() => null;
    }
}

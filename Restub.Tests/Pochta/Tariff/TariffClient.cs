using Restub.Toolbox;

namespace Restub.Tests.Pochta.Tariff
{
    /// <summary>
    /// Pochta.ru tariffication API sample client, requires no authentication.
    /// https://tariff.pochta.ru/
    /// </summary>
    public class TariffClient : RestubClient
    {
        public const string BaseUrl = "https://tariff.pochta.ru/";

        public TariffClient() : base(BaseUrl)
        {
        }

        protected override Authenticator CreateAuthenticator() => null;

        /// <summary>
        /// Calculates the tariff.
        /// </summary>
        /// <param name="request">Tariff calculation request.</param>
        /// <returns>Calculated tariff in the requested format.</returns>
        public TariffResponse Calculate(TariffRequest request) =>
            Get<TariffResponse>("v2/calculate/tariff", r => r
                .AddQueryParameter("json", "json")
                .AddQueryString(request));
    }
}

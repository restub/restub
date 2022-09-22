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

        private string GetFormat(TariffResponseFormat format) =>
            ParameterHelper.GetEnumMemberValue(format);

        /// <summary>
        /// Calculates the tariff.
        /// </summary>
        /// <param name="request">Tariff calculation request.</param>
        /// <returns>Calculated tariff in the requested format.</returns>
        public TariffResponse Calculate(TariffRequest request) =>
            Get<TariffResponse>("v2/calculate/tariff", r => r
                .AddQueryParameter("json", "json")
                .AddQueryString(request));

        /// <summary>
        /// Calculates the tariff and returns the.
        /// </summary>
        /// <param name="format">Tariff response format.</param>
        /// <param name="request">Tariff calculation request.</param>
        /// <returns>Calculated tariff in the requested format.</returns>
        public string Calculate(TariffResponseFormat format, TariffRequest request) =>
            Get("v2/calculate/tariff", r => r
                .AddQueryParameter(GetFormat(format), null)
                .AddQueryString(request));
    }
}

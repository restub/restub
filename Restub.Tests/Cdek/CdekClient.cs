using Restub.Toolbox;

namespace Restub.Tests.Cdek
{
    /// <summary>
    /// Sample CDEK API client.
    /// </summary>
    public class CdekClient : RestubClient
    {
        /// <summary>
        /// Sandbox API endpoint.
        /// </summary>
        public const string SandboxApiUrl = "https://api.edu.cdek.ru/v2/";

        /// <summary>
        /// Initializes a new instance of the <see cref="CdekClient"/> class.
        /// </summary>
        /// <param name="baseUrl">Base URL of CDEK REST API.</param>
        /// <param name="credentials">Client identifier and the secret.</param>
        public CdekClient(string baseUrl, CdekCredentials credentials)
            : base(baseUrl, credentials)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CdekClient"/> class
        /// with sandbox API URL and test credentials.
        /// </summary>
        public CdekClient()
            : this(SandboxApiUrl, CdekCredentials.TestCredentials)
        {
        }

        protected override Authenticator CreateAuthenticator() =>
            new CdekAuthenticator(this, (CdekCredentials)Credentials);

        protected override IRestubSerializer CreateSerializer() =>
            new CdekSerializer();

        /// <summary>
        /// Acquires a JWT token for the CDEK API.
        /// EN: https://api-docs.cdek.ru/33828799.html
        /// RU: https://api-docs.cdek.ru/29923918.html
        /// </summary>
        /// <param name="clientAccount">Account identifier.</param>
        /// <param name="clientSecret">Client secret or password.</param>
        internal CdekAuthToken GetAuthToken(string clientAccount, string clientSecret) =>
            Post<CdekAuthToken>("oauth/token?parameters", null, r =>
            {
                r.AlwaysMultipartFormData = true;
                r.AddParameters(new
                {
                    grant_type = "client_credentials",
                    client_id = clientAccount,
                    client_secret = clientSecret,
                });
            });

        /// <summary>
        /// Gets the list of regions.
        /// EN: https://api-docs.cdek.ru/33829453.html
        /// RU: https://api-docs.cdek.ru/33829418.html
        /// </summary>
        public CdekRegion[] GetRegions(int? size = null, int? page = null) =>
        	Get<CdekRegion[]>("location/regions", r => r.AddQueryString(new { size, page }));

        /// <summary>
        /// Gets the list of cities.
        /// EN: https://api-docs.cdek.ru/33829473.html
        /// RU: https://api-docs.cdek.ru/33829437.html
        /// </summary>
        public CdekCity[] GetCities(string[] countries = null, string city = null, int? size = null) =>
            Get<CdekCity[]>("location/cities", r => r.AddQueryString(new
            {
                city,
                country_codes = countries,
                size,
            }));
    }
}

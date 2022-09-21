using System;
using System.Linq;

namespace Restub.Tests.Pochta.Otpravka
{
    /// <summary>
    /// Pochta.ru Otpravka API sample client
    /// </summary>
    public class OtpravkaClient : RestubClient
    {
        public const string BaseUrl = "https://otpravka-api.pochta.ru";

        public OtpravkaClient(OtpravkaCredentials credentials)
            : base(BaseUrl, credentials)
        {
        }

        protected override Authenticator CreateAuthenticator() =>
            new OtpravkaAuthenticator(this, (OtpravkaCredentials)Credentials);

        /// <summary>
        /// Address normalization.
        /// https://otpravka.pochta.ru/specification#/nogroup-normalization_adress
        /// </summary>
        /// <param name="address">Address to normalize.</param>
        /// <returns>Normalized address.</returns>
        public OtpravkaAddress CleanAddress(string address) =>
            Post<OtpravkaAddress[]>("/1.0/clean/address", new[]
            {
                new OtpravkaAddressRequest
                {
                    ID = "adr1",
                    OriginalAddress = address,
                }
            })
            .FirstOrDefault();

        /// <summary>
        /// Address normalization, batch mode.
        /// https://otpravka.pochta.ru/specification#/nogroup-normalization_adress
        /// </summary>
        /// <param name="addresses">Addresses to normalize.</param>
        /// <returns>Normalized addresses, in the same order.</returns>
        public OtpravkaAddress[] CleanAddress(params string[] addresses)
        {
            var req = addresses.Select((a, i) => new OtpravkaAddressRequest
            {
                ID = i.ToString(),
                OriginalAddress = a,
            });

            var result = Post<OtpravkaAddress[]>("/1.0/clean/address", req.ToArray());

            // make sure that normalized addresses are returned in the same order
            return result.OrderBy(a => Convert.ToInt32(a.ID)).ToArray();
        }
    }
}

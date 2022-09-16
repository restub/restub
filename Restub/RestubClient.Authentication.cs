using Restub.DataContracts;
using Restub.Toolbox;

namespace Restub
{
    /// <remarks>
    /// Sample athentication primitives to be called by Credentials class.
    /// </remarks>
    public partial class RestubClient
    {
        ///// <summary>
        ///// Acquires a JWT token for the API.
        ///// </summary>
        ///// <param name="clientAccount">Account identifier.</param>
        ///// <param name="clientSecret">Client secret or password.</param>
        //internal AuthToken GetAuthToken(string clientAccount, string clientSecret)
        //{
        //    return Post<AuthToken>("oauth/token?parameters", null, r =>
        //    {
        //        r.AlwaysMultipartFormData = true;
        //        r.AddParameters(new
        //        {
        //            grant_type = "client_credentials",
        //            client_id = clientAccount,
        //            client_secret = clientSecret,
        //        });
        //    });
        //}
    }
}
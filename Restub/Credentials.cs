using Exprest.DataContracts;

namespace Exprest
{
    /// <summary>
    /// Abstract credentials.
    /// </summary>
    /// <remarks>
    /// Usage: subclass it, add user+password or client+secret properties,
    /// implement Authenticate method.
    /// </remarks>
    public class Credentials
    {
        /// <summary>
        /// Authenticates the client using account/secret pair.
        /// </summary>
        /// <param name="client">API client to make an authentication request.</param>
        public virtual AuthToken Authenticate(ExprestClient client)
        {
            // real API client will call something like:
            // client.GetAuthToken(ClientAccount, ClientSecret);
            return new AuthToken();
        }
    }
}

using Restub.DataContracts;

namespace Restub
{
    /// <summary>
    /// Abstract credentials.
    /// </summary>
    public abstract class Credentials
    {
        /// <summary>
        /// Authenticates the client using account/secret pair.
        /// </summary>
        /// <param name="client">API client to make an authentication request.</param>
        public abstract AuthToken Authenticate(RestubClient client);
    }
}

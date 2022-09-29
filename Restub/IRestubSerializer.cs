using RestSharp.Serialization;

namespace Restub
{
    /// <summary>
    /// Custom serializer interface with string deserialization support.
    /// </summary>
    public interface IRestubSerializer : IRestSerializer
    {
        /// <summary>
        /// Deserializes the given string into an object.
        /// </summary>
        /// <typeparam name="T">Target object type.</typeparam>
        /// <param name="json">Json string to deserialize.</param>
        /// <returns></returns>
        T Deserialize<T>(string json);
    }
}

using System;
using System.Reflection;
using RestSharp;

namespace Restub.Toolbox
{
    /// <summary>
    /// Helper extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Adds the given header if it's not empty.
        /// </summary>
        /// <param name="request"><see cref="IRestRequest"/> instance.</param>
        /// <param name="name">Header name.</param>
        /// <param name="value">Header value.</param>
        /// <returns>The same <see cref="IRestRequest"/> instance.</returns>
        public static IRestRequest AddHeaderIfNotEmpty(this IRestRequest request, string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(value))
            {
                return request.AddHeader(name, value);
            }

            return request;
        }

        /// <summary>
        /// Gets the version of the library of the given type.
        /// </summary>
        /// <typeparam name="T">The type to reflect on.</typeparam>
        /// <returns>Assembly version.</returns>
        public static string GetAssemblyVersion<T>() => typeof(T).GetAssemblyVersion();

        /// <summary>
        /// Gets the version of the library of the given type.
        /// </summary>
        /// <param name="type">The type to reflect on.</param>
        /// <returns>Assembly version.</returns>
        public static string GetAssemblyVersion(this Type type)
        {
            var asm = type.Assembly;
            var attr = (AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyFileVersionAttribute), false);
            return attr?.Version ?? asm.GetName().Version.ToString();
        }
    }
}

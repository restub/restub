using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Converts the given string to the title case.
        /// Example: "Hello, world" → "HelloWorld".
        /// </summary>
        /// <param name="s">String to convert.</param>
        /// <returns>Conversion result.</returns>
        public static string ToTitleCase(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }

            string clean(string p) =>
                string.Join(string.Empty, p.Where(char.IsLetterOrDigit));

            string titleCase(string p) =>
                string.IsNullOrWhiteSpace(p) ? string.Empty :
                    p.Substring(0, 1).ToUpperInvariant() + p.Substring(1).ToLowerInvariant();

            return string.Join(string.Empty,
                s.Split(' ', '-', '/', '\'', '"', '«', '»')
                    .Select(clean)
                    .Select(titleCase));
        }

        /// <summary>
        /// Removes duplicates from <see cref="IEnumerable{T}"/> based on the given key selector.
        /// </summary>
        /// <typeparam name="T">Enumerable sequence item type.</typeparam>
        /// <typeparam name="TKey">Enumerable sequence key type.</typeparam>
        /// <param name="enumerable">Enumerable sequence to filter.</param>
        /// <param name="keySelector">Key property selector callback.</param>
        /// <param name="comparer">Optional key comparer.</param>
        /// <returns>The same sequence without the duplicates.</returns>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer = null)
        {
            if (enumerable == null || !enumerable.Any())
            {
                yield break;
            }

            var keys = new HashSet<TKey>(comparer);
            foreach (var item in enumerable)
            {
                var key = keySelector(item);
                if (keys.Add(key))
                {
                    yield return item;
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using RestSharp;

namespace Restub.Toolbox
{
    /// <summary>
    /// Helper class to transform DTO classes into REST parameters.
    /// </summary>
    public static class ParameterHelper
    {
        /// <summary>
        /// Adds all dataContract properties to the given request as QueryString parameters.
        /// </summary>
        /// <param name="request">Request to populate parameters.</param>
        /// <param name="dataContract">Data contract to get the parameters from.</param>
        public static IRestRequest AddQueryString(this IRestRequest request, object dataContract) =>
            request.AddParameters(dataContract, ParameterType.QueryString);

        /// <summary>
        /// Adds all dataContract properties to the given request.
        /// </summary>
        /// <param name="request">Request to populate parameters.</param>
        /// <param name="dataContract">Data contract to get the parameters from.</param>
        /// <param name="type">Parameter type.</param>
        public static IRestRequest AddParameters(this IRestRequest request, object dataContract, ParameterType type = ParameterType.GetOrPost)
        {
            if (dataContract == null || dataContract.GetType().IsPrimitive)
            {
                return request;
            }

            var props = dataContract.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var p in props)
            {
                var value = p.GetValue(dataContract);
                var defaultValue = p.PropertyType.GetDefaultValue();
                var isNonDefault = !Equals(value, defaultValue);
                var isNullableType = p.PropertyType.IsNullable();
                var nonNullableType = p.PropertyType.GetNonNullableType();
                var isEnumValue = nonNullableType.IsEnum;
                var dataMember = p.GetCustomAttribute<DataMemberAttribute>();
                var isRequired = dataMember?.IsRequired ?? (isEnumValue && !isNullableType);

                if (isNonDefault || isRequired)
                {
                    // get parameter name from DataMember attribute
                    var parameterName = dataMember?.Name ?? p.Name;
                    if (value == null || value is string)
                    {
                        request.AddParameter(parameterName, value, type);
                        continue;
                    }

                    // get enum value from DataMember attribute
                    if (isEnumValue)
                    {
                        var valueName = GetEnumMemberValue(nonNullableType, value);
                        request.AddParameter(parameterName, valueName, type);
                        continue;
                    }

                    if (p.PropertyType.IsPrimitive)
                    {
                        request.AddParameter(parameterName, value, type);
                        continue;
                    }

                    // support array values like this: pages=1,2,3
                    if (value is IEnumerable enumerable)
                    {
                        value = string.Join(",", enumerable.OfType<object>());
                        request.AddParameter(parameterName, value, type);
                        continue;
                    }

                    request.AddParameter(parameterName, value, type);
                }
            }

            return request;
        }

        /// <summary>
        /// Returns the enumeration member value.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration</typeparam>
        /// <param name="enumValue">The value to inspect.</param>
        /// <returns>String representation of the value.</returns>
        public static string GetEnumMemberValue<T>(T enumValue) =>
            GetEnumMemberValue(typeof(T), enumValue);

        /// <summary>
        /// Returns the enumeration member value.
        /// </summary>
        /// <param name="nonNullableType">The type of the enumeration</param>
        /// <param name="value">The value to inspect.</param>
        /// <returns>String representation of the value.</returns>
        private static string GetEnumMemberValue(Type nonNullableType, object value)
        {
            var valueName = Enum.GetName(nonNullableType, value);
            if (valueName == null)
            {
                valueName = value.ToString();
            }

            var field = nonNullableType.GetField(valueName);
            var enumMember = field?.GetCustomAttribute<EnumMemberAttribute>();
            if (enumMember != null)
            {
                valueName = enumMember.Value;
            }

            return valueName;
        }

        /// <summary>
        /// Gets a value indicating whether the type is nullable.
        /// </summary>
        /// <param name="type">The type to examine.</param>
        public static bool IsNullable(this Type type) =>
            type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        /// <summary>
        /// Returns the underlying nullable type.
        /// </summary>
        /// <param name="type">The type to examine.</param>
        public static Type GetNonNullableType(this Type type) =>
            type.IsNullable() ? type.GetGenericArguments().First() : type;

        /// <summary>
        /// Gets the default value of the given type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        public static T GetDefaultValue<T>() => default(T);

        /// <summary>
        /// Gets the default value of the given type.
        /// </summary>
        /// <param name="type">The type.</param>
        public static object GetDefaultValue(this Type type)
        {
            Func<int> getDefault = GetDefaultValue<int>;
            var getDefaultMethod = getDefault.Method.GetGenericMethodDefinition();
            return getDefaultMethod.MakeGenericMethod(type).Invoke(null, null);
        }
    }
}

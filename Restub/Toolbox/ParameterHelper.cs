using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using RestSharp;
using RestSharp.Serialization;
using Restub.DataContracts;

namespace Restub.Toolbox
{
    /// <summary>
    /// Helper class to transform DTO classes into REST parameters.
    /// </summary>
    public static class ParameterHelper
    {
        /// <summary>
        /// Default serializer to be used when a request doesn't have one.
        /// </summary>
        public static IRestubSerializer DefaultSerializer { get; set; } =
            new NewtonsoftSerializer();

        /// <summary>
        /// Adds all dataContract properties to the given request as QueryString parameters.
        /// </summary>
        /// <param name="request">Request to populate parameters.</param>
        /// <param name="dataContract">Data contract to get the parameters from.</param>
        /// <param name="mode">Enum values serialization mode.</param>
        public static IRestRequest AddQueryString(this IRestRequest request, object dataContract, EnumSerializationMode mode = EnumSerializationMode.Auto) =>
            request.AddParameters(dataContract, ParameterType.QueryString, mode);

        /// <summary>
        /// Adds all dataContract properties to the given request.
        /// </summary>
        /// <param name="request">Request to populate parameters.</param>
        /// <param name="dataContract">Data contract to get the parameters from.</param>
        /// <param name="type">Parameter type.</param>
        /// <param name="mode">Enum values serialization mode.</param>
        public static IRestRequest AddParameters(this IRestRequest request, object dataContract, ParameterType type = ParameterType.GetOrPost, EnumSerializationMode mode = EnumSerializationMode.Auto)
        {
            if (dataContract == null || dataContract.GetType().IsPrimitive)
            {
                return request;
            }

            var props = dataContract.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var p in props)
            {
                if (p.IsDefined(typeof(IgnoreDataMemberAttribute)))
                {
                    continue;
                }

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

                    // get enum value from EnumMember attribute
                    if (isEnumValue)
                    {
                        var valueName = GetEnumMemberValue(nonNullableType, value, mode);
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
                        var enumObjects = enumerable.OfType<object>();
                        var stringObjects = enumObjects.Select(o =>
                            o != null && o.GetType().IsEnum ?
                                GetEnumMemberValue(o) : $"{o}");

                        value = string.Join(",", stringObjects);
                        request.AddParameter(parameterName, value, type);
                        continue;
                    }

                    // convert to string
                    var serializer = request.JsonSerializer ?? DefaultSerializer;
                    var stringValue = serializer.Serialize(value);
                    request.AddParameter(parameterName, stringValue.Trim('"'), type);
                }
            }

            return request;
        }

        /// <summary>
        /// Returns the enumeration member value.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration</typeparam>
        /// <param name="enumValue">The value to inspect.</param>
        /// <param name="mode">Enum values serialization mode.</param>
        /// <returns>String or integer representation of the value.</returns>
        public static object GetEnumMemberValue<T>(T enumValue, EnumSerializationMode mode = EnumSerializationMode.Auto) =>
            GetEnumMemberValue(typeof(T), enumValue, mode);

        /// <summary>
        /// Returns the enumeration member value.
        /// </summary>
        /// <param name="nonNullableType">The type of the enumeration</param>
        /// <param name="value">The value to inspect.</param>
        /// <param name="mode">Enum values serialization mode.</param>
        /// <returns>String or integer representation of the value.</returns>
        private static object GetEnumMemberValue(Type nonNullableType, object value, EnumSerializationMode mode = EnumSerializationMode.Auto)
        {
            // auto = serialize as integer if not marked as DataContract
            if ((mode == EnumSerializationMode.Number) ||
                (mode == EnumSerializationMode.Auto && 
                !nonNullableType.IsDefined(typeof(DataContractAttribute))))
            {
                return Convert.ToInt32(value);
            }

            // serialize as string
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
            (type?.IsNullable() ?? false) ? type.GetGenericArguments().First() : type;

        /// <summary>
        /// Gets the default value of the given type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        public static T GetDefaultValue<T>() => default;

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

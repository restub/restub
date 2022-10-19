using System;
using System.Linq;
using Restub.Toolbox;

namespace Restub.DataContracts
{
    /// <summary>
    /// Specifies the default enum member value for deserialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class DefaultEnumMemberAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEnumMemberAttribute"/> class.
        /// </summary>
        /// <param name="defaultValue">Default value of the enum.</param>
        public DefaultEnumMemberAttribute(object defaultValue)
        {
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Gets the default value for the enumeration.
        /// </summary>
        public object DefaultValue { get; }

        /// <summary>
        /// Gets the default value for the given enum.
        /// </summary>
        /// <param name="enumType">The type of the enum.</param>
        /// <returns>Default value, if specified, or null.</returns>
        public static object GetDefaultValue(Type enumType)
        {
            enumType = enumType.GetNonNullableType();
            if (enumType == null || enumType.IsEnum == false)
            {
                return null;
            }

            var attr = enumType
                .GetCustomAttributes(typeof(DefaultEnumMemberAttribute), false)
                .OfType<DefaultEnumMemberAttribute>()
                .FirstOrDefault();

            var defaultValue = attr?.DefaultValue;
            if (defaultValue != null && defaultValue.GetType() == enumType)
            {
                return defaultValue;
            }

            return null;
        }
    }
}

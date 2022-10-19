using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Restub.DataContracts;

namespace Restub.Toolbox
{
    /// <summary>
    /// Enum converter that supports both string and numeric representations of enums.
    /// </summary>
    public class AutoEnumConverter : StringEnumConverter
    {
        /// <iheritdocs/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var e = (Enum)value;
            var isDataContract = e.GetType().IsDefined(typeof(DataContractAttribute), false);
            if (isDataContract)
            {
                base.WriteJson(writer, value, serializer);
                return;
            }

            // enum should be serialized as number
            writer.WriteValue(value);
        }

        /// <iheritdocs/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch (JsonSerializationException)
            {
                // if we can't deserialize an unknown enum member, return the default value
                var value = DefaultEnumMemberAttribute.GetDefaultValue(objectType);
                if (value != null)
                {
                    return value;
                }

                throw;
            }
        }
    }
}

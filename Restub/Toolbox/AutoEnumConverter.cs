using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Restub.Toolbox
{
    public class AutoEnumConverter : StringEnumConverter
    {
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
    }
}

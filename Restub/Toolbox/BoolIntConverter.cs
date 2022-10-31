using System;
using Newtonsoft.Json;

namespace Restub.Toolbox
{
    /// <summary>
    /// Handles converting non-boolean JSON values into a C# boolean data type.
    /// </summary>
    /// <remarks>
    /// Based on: https://gist.github.com/randyburden/5924981,
    /// added serialization part and nullable boolean support.
    /// </remarks>
    public class BoolIntConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType) =>
            objectType.GetNonNullableType() == typeof(bool);

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null && objectType.IsNullable())
            {
                return null;
            }

            if (reader.TokenType == JsonToken.Integer)
            {
                return Convert.ToInt32(reader.Value) != 0;
            }

            switch (reader.Value.ToString().ToLower().Trim())
            {
                case "true":
                case "yes":
                case "y":
                case "1":
                    return true;

                case "false":
                case "no":
                case "n":
                case "0":
                    return false;
            }

            // let Json.NET throw
            return new JsonSerializer().Deserialize(reader, objectType);
        }

        /// <summary>
        /// Specifies that this converter will participate in writing results.
        /// </summary>
        public override bool CanWrite => true;

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
            }
            else if (value is true)
            {
                writer.WriteValue(1);
            }
            else if (value is false)
            {
                writer.WriteValue(0);
            }
            else
            {
                throw new JsonSerializationException($"Unexpected boolean value: {value}");
            }
        }
    }
}

using System;
using System.Globalization;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization;

namespace Restub.Toolbox
{
    /// <summary>
    /// Newtonsoft.Json serializer.
    /// </summary>
    public class NewtonsoftSerializer : IRestSerializer
    {
        public string[] SupportedContentTypes { get; } =
        {
            "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
        };

        public DataFormat DataFormat => DataFormat.Json;

        public string ContentType { get; set; } = "application/json";

        public JsonSerializerSettings Settings { get; } = CreateJsonSerializerSettings();

        private static JsonSerializerSettings CreateJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            settings.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter
            {
                // for some reason, CDEK API requires that time zone
                // doesn't have a colon, expecting "+0700" instead of "+07:00"
                DateTimeFormat = @"yyyy-MM-dd\THH:mm:sszz00",
                DateTimeStyles = DateTimeStyles.AllowWhiteSpaces,
            });

            return settings;
        }

        public T Deserialize<T>(IRestResponse response) =>
            JsonConvert.DeserializeObject<T>(response.Content, Settings);

        public string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter.Value, Settings);

        public string Serialize(object obj) =>
            JsonConvert.SerializeObject(obj, Settings);

        public T Deserialize<T>(string json) =>
            JsonConvert.DeserializeObject<T>(json, Settings);
    }
}

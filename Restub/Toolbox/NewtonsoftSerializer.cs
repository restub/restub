using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;

namespace Restub.Toolbox
{
    /// <summary>
    /// Newtonsoft.Json serializer.
    /// </summary>
    public class NewtonsoftSerializer : IRestubSerializer
    {
        public string[] SupportedContentTypes { get; } =
        {
            "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
        };

        public DataFormat DataFormat => DataFormat.Json;

        public string ContentType { get; set; } = "application/json";

        private JsonSerializerSettings settings;

        public JsonSerializerSettings Settings => settings ??
            (settings = CreateJsonSerializerSettings());

        protected virtual JsonSerializerSettings CreateJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
            settings.Converters.Add(new AutoEnumConverter());
            settings.Converters.Add(new IsoDateTimeConverter
            {
                DateTimeFormat = @"yyyy-MM-dd\THH:mm:sszzzz",
                DateTimeStyles = DateTimeStyles.AllowWhiteSpaces,
            });

            return settings;
        }

        public virtual T Deserialize<T>(IRestResponse response) =>
            JsonConvert.DeserializeObject<T>(response.Content, Settings);

        public virtual string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter.Value, Settings);

        public virtual string Serialize(object obj) =>
            JsonConvert.SerializeObject(obj, Settings);

        public virtual T Deserialize<T>(string json) =>
            JsonConvert.DeserializeObject<T>(json, Settings);
    }
}

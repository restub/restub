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
        /// <inheritdoc/>
        public string[] SupportedContentTypes { get; } =
        {
            "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
        };

        /// <inheritdoc/>
        public DataFormat DataFormat => DataFormat.Json;

        /// <inheritdoc/>
        public string ContentType { get; set; } = "application/json";

        private JsonSerializerSettings settings;

        /// <summary>
        /// Gets JSON serializer settings.
        /// </summary>
        public JsonSerializerSettings Settings => settings ??
            (settings = CreateJsonSerializerSettings());

        /// <summary>
        /// Creates JSON serializer settings to be used.
        /// </summary>
        protected virtual JsonSerializerSettings CreateJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind
            };

            settings.Converters.Add(new AutoEnumConverter());
            settings.Converters.Add(new IsoDateTimeConverter
            {
                DateTimeFormat = @"yyyy-MM-dd\THH:mm:sszzzz",
                DateTimeStyles = DateTimeStyles.AllowWhiteSpaces,
            });

            return settings;
        }

        /// <inheritdoc/>
        public virtual T Deserialize<T>(IRestResponse response) =>
            JsonConvert.DeserializeObject<T>(response.Content, Settings);

        /// <inheritdoc/>
        public virtual string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter.Value, Settings);

        /// <inheritdoc/>
        public virtual string Serialize(object obj) =>
            JsonConvert.SerializeObject(obj, Settings);

        /// <inheritdoc/>
        public virtual T Deserialize<T>(string json) =>
            JsonConvert.DeserializeObject<T>(json, Settings);
    }
}

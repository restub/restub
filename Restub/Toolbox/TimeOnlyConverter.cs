using Newtonsoft.Json.Converters;

namespace Restub.Toolbox
{
    /// <summary>
    /// Some date properties have only the time part.
    /// </summary>
    public class TimeOnlyConverter : IsoDateTimeConverter
    {
        public TimeOnlyConverter()
        {
            DateTimeFormat = "HH:mm";
        }
    }
}

using Newtonsoft.Json.Converters;

namespace Restub.Toolbox
{
    /// <summary>
    /// Some date properties have only the time part.
    /// </summary>
    public class TimeOnlyConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOnlyConverter"/> class.
        /// </summary>
        public TimeOnlyConverter()
        {
            DateTimeFormat = "HH:mm";
        }
    }
}

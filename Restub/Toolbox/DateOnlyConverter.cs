using Newtonsoft.Json.Converters;

namespace Restub.Toolbox
{
    /// <summary>
    /// Some date properties don't have the time.
    /// </summary>
    public class DateOnlyConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateOnlyConverter"/> class.
        /// </summary>
        public DateOnlyConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}

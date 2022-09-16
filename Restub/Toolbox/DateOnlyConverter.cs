using Newtonsoft.Json.Converters;

namespace Restub.Toolbox
{
    /// <summary>
    /// Some date properties don't have the time.
    /// </summary>
    public class DateOnlyConverter : IsoDateTimeConverter
    {
        public DateOnlyConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}

namespace Domain.Helpers
{
    public static class Converters
    {
        public static DateTime? ToDateTimeUTC(this string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            DateTime date;

            var success = DateTime.TryParse(dateString, out date);

            if (success)
                return date.ToUniversalTime();

            return null;
        }

        public static string ToStringDateTime(this DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return dateTime.ToString(format);
        }
    }
}

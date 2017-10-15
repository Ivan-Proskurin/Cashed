using System;
using System.Globalization;

namespace Cashed.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly CultureInfo culture = new CultureInfo("ru-ru");

        public static DateTime StartOfTheMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        public static DateTime EndOfTheMonth(this DateTime value)
        {
            return StartOfTheMonth(value).AddMonths(1);
        }

        public static string ToStandardString(this DateTime value, bool toMinutes = true)
        {
            return value.ToString(toMinutes ? "yyyy.MM.dd HH:mm" : "yyyy.MM.dd HH:00");
        }

        public static string ToStandardDateStr(this DateTime value)
        {
            return value.ToString("yyyy.MM.dd");
        }

        public static DateTime ParseDtFromStandardString(this string value)
        {
            return DateTime.ParseExact(value, "yyyy.MM.dd HH:mm", culture);
        }
    }
}
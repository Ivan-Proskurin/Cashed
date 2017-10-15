using System.Globalization;

namespace Cashed.Extensions
{
    public static class DecimalExtension
    {
        public static decimal ParseMoneyInvariant(this string value)
        {
            return decimal.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture);
        }
    }
}

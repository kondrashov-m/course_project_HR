using System;

namespace HRSystem.Utils
{
    public enum Currency
    {
        RUB,
        USD
    }

    public static class AppSettings
    {
        public static Currency Currency { get; set; } = Currency.RUB;

        public static string FormatMoney(decimal amount)
        {
            switch (Currency)
            {
                case Currency.USD: return $"{amount:F2} $";
                default: return $"{amount:F2} ₽";
            }
        }
    }
}

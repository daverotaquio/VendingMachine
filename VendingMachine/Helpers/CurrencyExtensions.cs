using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace VendingMachine.Helpers
{
    public static class CurrencyExtensions
    {
        public static string ToCurrency(this decimal amount)
        {
            return $"P{amount}";
        }
    }
}

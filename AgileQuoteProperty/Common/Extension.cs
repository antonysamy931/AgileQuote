using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AgileQuoteProperty.Common
{
    public static class Extension
    {
        public static Regex myRegex = new Regex("[^0-9a-zA-Z.]+");
        public static string Expression = "[^0-9a-zA-Z.]+";

        public static decimal StringToDecimal(string Value)
        {
            if (string.IsNullOrEmpty(Value))
            {
                return ConstantValues.Zero;
            }
            if (myRegex.IsMatch(Value))
            {
                string TempValue = RemoveSpecialCharacter(Value);
                return Convert.ToDecimal(TempValue);
            }
            else
            {
                return Convert.ToDecimal(Value);
            }
        }

        public static decimal StringCurrencyToDecimal(string Value)
        {
            if (Value == null)
            {
                return ConstantValues.Zero;
            }
            var TempValue = Value.Substring(1);
            return Convert.ToDecimal(TempValue);
        }

        public static string DecimalToString(decimal Value)
        {            
            return Convert.ToString(Value);
        }

        public static decimal IntToDecimal(int Value)
        {           
            return Convert.ToDecimal(Value);
        }

        public static String FormatCurrency(NumberFormatInfo nfi, object Value)
        {
            try
            {
                return String.Format(nfi, "{0:C}", Value);
            }
            catch
            {
                throw;
            }
        }

        public static decimal RoundDecimal(decimal Value)
        {
            decimal RoundValue = Math.Round(Value);
            string Temp = Convert.ToString(RoundValue).PadLeft(4, '0');
            return Convert.ToDecimal(Temp);
        }

        public static string RemoveSpecialCharacter(string Value)
        {
            return Regex.Replace(Value, Expression, "");
        }

        public static int MonthConverter(decimal Warrenty)
        {
            try
            {
                var splitWarrenty = Warrenty.ToString().Split('.');
                int yearCal = Convert.ToInt32(splitWarrenty[0]) * 12;
                int monthCal = 0;
                if (Convert.ToInt32(splitWarrenty[1]) > 12)
                {
                    var month = splitWarrenty[1];
                    monthCal = Convert.ToInt32(month.Substring(0, 1));
                }
                else
                {
                    monthCal = Convert.ToInt32(splitWarrenty[1]);
                }

                return yearCal + monthCal;
            }
            catch
            {
                throw;
            }
        }

        public static decimal YearConverter(int Value)
        {
            try
            {
                var Month = Value % 12;
                var Year = Value / 12;
                return Convert.ToDecimal(Year + "." + Month);
            }
            catch
            {
                throw;
            }
        }

        public static decimal MathRound(decimal Value)
        {
            try
            {
                return Math.Round(Value);
            }
            catch
            {
                throw;
            }
        }

        public static string DateFormat(DateTime Value)
        {
            try
            {
                return String.Format("{0:dd-MMM-yyyy}", Value);
            }
            catch
            {
                throw;
            }
        }
    }
}

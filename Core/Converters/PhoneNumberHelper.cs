namespace MobileApp.Core.Converters
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Number util
    /// </summary>
    public static class PhoneNumberHelper
    {
        /// <summary>
        /// Checks if number is big one (like +316.....)
        /// </summary>
        /// <param name="number">
        /// Number
        /// </param>
        /// <returns>
        /// Result
        /// </returns>
        public static bool IsExternal(string number)
        {
            return number.Length > 4;
        }

        /// <summary>
        /// Trim number and provide it to short format
        /// </summary>
        /// <param name="number">Some number</param>
        /// <returns>Trimed and shortened number</returns>
        public static string ToComparable(string number)
        {
            const string RegexPattern = @"^31|^0"; 
            return Regex.Replace(number.Trim(new[] { ' ', '+' }), RegexPattern, string.Empty);
        }

        /// <summary>
        /// Compare 2 dutch numbers
        /// </summary>
        /// <param name="number1">
        /// Number1 
        /// </param>
        /// <param name="number2">
        /// Number2
        /// </param>
        /// <returns>
        /// Equality result
        /// </returns>
        public static bool AreEqual(string number1, string number2)
        {
            if (string.IsNullOrWhiteSpace(number1) || string.IsNullOrWhiteSpace(number2))
            {
                return false;
            }

            if (number1.Equals(number2, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            number1 = number1.Trim(new[] { ' ', '+' });
            number2 = number2.Trim(new[] { ' ', '+' });
            const string RegexPattern = @"^31|^0";
            number1 = Regex.Replace(number1, RegexPattern, string.Empty);
            number2 = Regex.Replace(number2, RegexPattern, string.Empty);

            if (number1.Equals(number2, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Convert number to short format
        /// </summary>
        /// <param name="number">
        /// Number
        /// </param>
        /// <returns>
        /// Converted number
        /// </returns>
        public static string ToShort(string number)
        {
            number = (number ?? string.Empty).Trim();

            number = number.Replace("+", string.Empty);

            if (number.StartsWith("31"))
            {
                return number.Remove(0, 2).Insert(0, "0");
            }

            return number;
        }

        /// <summary>
        /// Convert number to full with plus
        /// </summary>
        /// <param name="number">
        /// Number
        /// </param>
        /// <returns>
        /// Converted number
        /// </returns>
        public static string ToFullWithPlus(string number)
        {
            number = (number ?? string.Empty).Trim();

            if (number.StartsWith("+"))
            {
                return number;
            }

            if (number.StartsWith("0"))
            {
                return number.Remove(0, 1).Insert(0, "+31");
            }

            return number.Insert(0, "+");
        }

        /// <summary>
        /// Convert number to number without plus
        /// </summary>
        /// <param name="number">
        /// Number
        /// </param>
        /// <returns>
        /// Converted number
        /// </returns>
        public static string ToNoPlus(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return number;
            }

            if (number.IndexOf("+", StringComparison.CurrentCultureIgnoreCase) < 0)
            {
                return number;
            }

            return number.Replace("+", string.Empty);
        }
    }
}
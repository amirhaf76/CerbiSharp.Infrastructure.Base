using CerbiSharp.Infrastructure.Base.RegexHelper;
using System.Text.RegularExpressions;

namespace CerbiSharp.Infrastructure.Base.Tools
{
    public static class HelperExtension
    {
        public static bool AreSameDateTimeWithSomeDistance(
            this DateTime firstDateTime,
            DateTime secondDateTime,
            TimeSpan maxDistanceTimeSpan
            )
        {
            if (firstDateTime.Date != secondDateTime.Date)
            {
                return false;
            }

            var distance = (firstDateTime.TimeOfDay - secondDateTime.TimeOfDay).Duration();

            return distance <= maxDistanceTimeSpan;
        }

        public static bool AreSameDateTimeWithSomeDistance(
            this DateTime firstDateTime,
            DateTimeOffset secondDateTime,
            TimeSpan maxDistanceTimeSpan
            )
        {
            return firstDateTime.AreSameDateTimeWithSomeDistance(secondDateTime.DateTime, maxDistanceTimeSpan);
        }

        public static bool AreSameDateTimeWithSomeDistance(
            this DateTimeOffset firstDateTime,
            DateTimeOffset secondDateTime,
            TimeSpan maxDistanceTimeSpan
            )
        {
            return firstDateTime.DateTime.AreSameDateTimeWithSomeDistance(secondDateTime.DateTime, maxDistanceTimeSpan);
        }

        /// <summary>
        /// Default duration distance time is 2 minutes.
        /// </summary>
        public static bool AreSameDateTimeWithSomeDistance(
            this DateTime firstDateTime,
            DateTime secondDateTime
            )
        {
            return firstDateTime.AreSameDateTimeWithSomeDistance(secondDateTime, TimeSpan.FromMinutes(2));
        }

        public static bool AreSameDateTimeWithSomeDistance(
            this DateTime firstDateTime,
            DateTimeOffset secondDateTime
            )
        {
            return firstDateTime.AreSameDateTimeWithSomeDistance(secondDateTime.DateTime, TimeSpan.FromMinutes(2));
        }

        public static bool AreSameDateTimeWithSomeDistance(
            this DateTimeOffset firstDateTime,
            DateTimeOffset secondDateTime
            )
        {
            return firstDateTime.DateTime.AreSameDateTimeWithSomeDistance(secondDateTime.DateTime, TimeSpan.FromMinutes(2));
        }

        /// <summary>
        /// Extract two number of interval.
        /// <code>GetInterval("0 - 43") == (0, 43)</code>
        /// </summary>
        /// <param name="input">must be contain two numbers, and parser reads from left to right.
        /// Format: {left number} - {right number}</param>
        /// <returns>Numbers are left side and right side respectively.</returns>
        /// <exception cref="ArgumentException">{nameof(input)} must be contain two numbers.</exception>
        public static (decimal, decimal) GetInterval(this string input)
        {
            MatchCollection intervalMatches = input.ExtractNumbers();

            if (intervalMatches.Count != 2)
            {
                throw new ArgumentException($"{nameof(input)} must be contain two numbers.");
            }

            decimal leftSide = decimal.Parse(intervalMatches[0].Value);
            decimal rightSide = decimal.Parse(intervalMatches[1].Value);

            return (leftSide, rightSide);
        }

        public static Match ExtractNumberAsMatch(this string input)
        {
            return Regex.Match(input, RegexPatterns.NUMBER_REGEX_PATTERN);
        }

        public static MatchCollection ExtractNumbers(this string input)
        {
            return Regex.Matches(input, RegexPatterns.NUMBER_REGEX_PATTERN);
        }

        public static TimeSpan ExtractTimeSpan(this string input)
        {
            string timeSpanPhrase = input.ExtractTimeSpanAsMatch().Value;

            return TimeSpan.Parse(timeSpanPhrase);
        }

        private static Match ExtractTimeSpanAsMatch(this string input)
        {
            return Regex.Match(input, @"(?'d'\d+\.)?(?:(?'h'2[0-3]|[0-1]\d):(?'m'[0-5]\d)(?:\:(?'s'\d{2}))?)(?'ms'\.\d+)?");
        }


        public static decimal RoundingNumberToNearestNumberFromLeft(this decimal number, int point)
        {
            long newNumber = decimal.ToInt64(decimal.Round(number));

            return newNumber.RoundingNumberToNearestNumberFromLeft(point);
        }

        public static double RoundingNumberToNearestNumberFromLeft(this double number, int point)
        {
            long newNumber = (long)Math.Round(number);

            return newNumber.RoundingNumberToNearestNumberFromLeft(point);
        }

        public static long RoundingNumberToNearestNumberFromLeft(this long number, int nthNumberFromLeft)
        {
            int numberLength = number.CountingDigitNumber();

            if (numberLength - nthNumberFromLeft < 0 || nthNumberFromLeft < 0)
            {
                throw new ArgumentException(
                    $"{nameof(nthNumberFromLeft)} argument must be positive." +
                    $" nthNumberFromLeft == {nthNumberFromLeft}"
                    );
            }

            if (nthNumberFromLeft == 0)
            {
                return number;
            }
            ;

            double shiftedToLeftNumber = ShiftNumberToRight(number, nthNumberFromLeft, numberLength);

            long roundedAfterShifting = (long)Math.Round(shiftedToLeftNumber);

            return (long)ShiftNumberToLeft(roundedAfterShifting, nthNumberFromLeft, numberLength);
        }

        private static double ShiftNumberToRight(long number, int nthNumberFromLeft, int numberLength)
        {
            return number * Math.Pow(0.1, numberLength - nthNumberFromLeft);
        }

        private static double ShiftNumberToLeft(long number, int nthNumberFromLeft, int numberLength)
        {
            return number * Math.Pow(10, numberLength - nthNumberFromLeft);
        }

        public static int CountingDigitNumber(this long number)
        {
            if (number < 0)
            {
                number *= -1;
            }

            if (number == 0)
            {
                return 1;
            }

            return (int)Math.Floor(Math.Log10(number) + 1);
        }

        public static Match ExtractIP(this string input)
        {
            return Regex.Match(input, RegexPatterns.IP_REGEX_PATTERN);
        }

        public static Match ExtractShebaNumber(this string input)
        {
            return Regex.Match(input, RegexPatterns.SHEBA_NUMBER_PATTERN);
        }

        public static decimal TruncateFractionalPartTill(this decimal input, int fractionalNumber)
        {
            double value = decimal.ToDouble(input).TruncateFractionalPartTill(fractionalNumber);

            return new decimal(value);
        }

        public static double TruncateFractionalPartTill(this double input, int fractionalNumber)
        {
            if (fractionalNumber < 0)
            {
                throw new Exception("Fractional number can not be lesser than zero!");
            }

            return input - input % Math.Pow(10, -1 * fractionalNumber);
        }

        public static decimal Normalize(this decimal input)
        {
            return input / 1.000000000000000000m;
        }


        //======

        public static void IfNotNullSetValue<T>(ref T parameter, T value, string parameterName)
        {
            if (value is null)
            {
                throw new Exception($"{parameterName} value can not be null!");
            }

            parameter = value;

        }

        public static T GetValue<T>(this IDictionary<string, object> dictiomary, string key)
        {
            object value = dictiomary[key];

            if (value is T result)
            {
                return result;
            }

            throw new InvalidCastException($"{value.GetType()} can not cast to {typeof(T)}.");
        }

        public static TValue GetValue<Tkey, TValue>(this IDictionary<Tkey, object> dictiomary, Tkey key)
        {
            object value = dictiomary[key];

            if (value is TValue result)
            {
                return result;
            }

            throw new InvalidCastException($"{value.GetType()} can not cast to {typeof(TValue)}.");
        }

        public static void SetOtpCodeHeader(HttpClient httpClient, string value)
        {
            httpClient.DefaultRequestHeaders.Add("otpCode", value);
        }

        public static void SetOtpTokenHeader(HttpClient httpClient, string value)
        {
            httpClient.DefaultRequestHeaders.Add("otpToken", value);
        }

        public static void SetPurposeHeader(HttpClient httpClient, string value)
        {
            httpClient.DefaultRequestHeaders.Add("purpose", value);
        }

        public static void SetTotpCodeHeader(HttpClient httpClient, string value)
        {
            httpClient.DefaultRequestHeaders.Add("totp", value);
        }

        public static void ProtectHttpClient(
            HttpClient httpClient,
            string otpCode,
            string otpToken,
            string totpCode,
            string purpose
            )
        {
            SetOtpCodeHeader(httpClient, otpCode);
            SetOtpTokenHeader(httpClient, otpToken);
            SetPurposeHeader(httpClient, purpose);
            SetTotpCodeHeader(httpClient, totpCode);
        }



        //public static Dictionary<string, string> GetKeyValueFromHtml(string html, string xPath, List<string> names)
        //{
        //    var htmlDoc = new HtmlDocument();

        //    htmlDoc.LoadHtml(html);

        //    HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes(xPath);

        //    IEnumerable<KeyValuePair<string, string>> keyValuePairs = nodes.
        //        Where(n =>
        //            names.Contains(n.GetAttributeValue("name", string.Empty))).
        //        Select(n => KeyValuePair.Create(
        //            n.GetAttributeValue("name", string.Empty),
        //            n.GetAttributeValue("value", string.Empty)
        //            ));

        //    return new Dictionary<string, string>(keyValuePairs);
        //}


    }
}

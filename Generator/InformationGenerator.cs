using System.Text;

namespace CerbiSharp.Infrastructure.Base.Generator
{
    public class InformationGenerator
    {
        public const string DateTimeIdGeneratorFormat = "yy_MM_dd_HH_mm";
        public const string DefaultFirstNameSeed = "نام کاربر";
        public const string DefaultLastNameSeed = "فامیلی کاربر";
        public const string InvaidNationalCodeSample = "0468364537";

        public static bool IsValidNationalId(string nationalCode)
        {
            return IsValidNationalId(long.Parse(nationalCode));
        }

        public static bool IsValidNationalId(long nationalCode)
        {
            (long ExclusiveNumberSeries, long ControllerNumber) = SeperateExclusiveNumberSeriesAndControlNumber(nationalCode);

            int calculatedControlNumber = CalculateControlNumber(ExclusiveNumberSeries);

            return ControllerNumber == calculatedControlNumber;
        }

        public static (long RxclusiveNumberSeries, long ControllerNumber) SeperateExclusiveNumberSeriesAndControlNumber(long nationalCode)
        {
            long controllerNumber = nationalCode % 10;

            long restOfNationalCode = (nationalCode - controllerNumber) / 10;

            return (restOfNationalCode, controllerNumber);
        }

        private static int CalculateControlNumber(string exclusiveNumberSeries)
        {
            return CalculateControlNumber(long.Parse(exclusiveNumberSeries));
        }

        private static int CalculateControlNumber(long exclusiveNumberSeries)
        {
            // It's a gard for invalid input.
            if (exclusiveNumberSeries < 0)
            {
                return -1;
            }

            int place = 2;

            int aggregateNumber = CalculateAggregateNumber(exclusiveNumberSeries, place);

            int controlNumberCheck = aggregateNumber % 11;

            if (controlNumberCheck < 2)
            {
                return controlNumberCheck;
            }
            else
            {
                return 11 - controlNumberCheck;
            }
        }

        private static int CalculateAggregateNumber(long exclusiveNumberSeries, int place)
        {
            int aggregateNumber = 0;

            while (exclusiveNumberSeries > 0)
            {
                int temp = (int)(exclusiveNumberSeries % 10);

                aggregateNumber += temp * place;
                place++;

                exclusiveNumberSeries = (exclusiveNumberSeries - temp) / 10;
            }

            return aggregateNumber;
        }

        public static string GenerageNationalId()
        {
            var time = DateTime.Now;

            var exclusiveNumberSeries = $"{time.Year % 10}{time:MMdd}{time:hhmm}";

            int controlNumber = CalculateControlNumber(exclusiveNumberSeries);

            return $"{exclusiveNumberSeries}{controlNumber}";
        }

        public static string GenerageInvalidNationalId()
        {
            var time = DateTime.Now;

            var exclusiveNumberSeries = $"{time.Year % 10}{time:MMdd}{time:hhmm}";

            int controlNumber = CalculateControlNumber(exclusiveNumberSeries);

            int invalidControlNumber = (1 + controlNumber) % 10;

            return $"{exclusiveNumberSeries}{invalidControlNumber}";
        }

        public static string GenerateMobileNumber()
        {
            var randomNum = new Random((int)(DateTime.Now.Ticks % int.MaxValue));

            var stringBuilder = new StringBuilder("989");

            for (int i = 0; i < 9; ++i)
            {
                stringBuilder.Append(randomNum.Next(0, 10));
            }

            return stringBuilder.ToString();
        }

        public static List<string> GenerateNameBaseOnDateTime(params string[] names)
        {
            string dateTime = DateTime.Now.ToString(DateTimeIdGeneratorFormat);

            var nameList = new List<string>(names.Length);

            foreach (var name in names)
            {
                nameList.Add(name + dateTime);
            }

            return nameList;
        }

        public static string GenerateFirstName()
        {
            return GenerateNameBaseOnDateTime(DefaultFirstNameSeed).First();
        }

        public static string GenerateSecondName()
        {
            return GenerateNameBaseOnDateTime(DefaultLastNameSeed).First();
        }

        public static string GenerateNextSimplePassword(char c)
        {
            if (!char.IsLetter(c))
            {
                throw new Exception($"The input parameter must be an letter character. The input is {c} character!");
            }

            if (char.ToLower(c) == 'z')
            {
                c = 'a';
            }
            else
            {
                c = (char)(c + 1);
            }

            return GenerateSimplePassword(c);
        }

        public static string GenerateSimplePassword(char c)
        {
            if (!char.IsLetter(c))
            {
                throw new Exception($"The input parameter must be an letter character. The input is {c} character!");
            }

            string passwordBase = char.IsUpper(c)
                ? $"{char.ToLower(c)}{c}"
                : $"{c}{char.ToUpper(c)}";

            return passwordBase + "123456";
        }

        public static string GenerateEmail(string username)
        {
            return $"{username}@HitobitEmail.com";
        }

        public static string GenerateEmail()
        {
            return GenerateEmail($"user_{DateTime.Now.ToString(DateTimeIdGeneratorFormat)}");
        }
    }
}

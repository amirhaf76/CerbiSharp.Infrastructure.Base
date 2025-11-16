using System.Text;

namespace CerbiSharp.Infrastructure.Base.Tools
{
    public static class Helper
    {
        public static void SaveDataAsTextFile(string data, string fileName, string directory)
        {
            string fullName = Path.Combine(directory, $"{fileName}.txt");

            using var newFile = File.CreateText(fullName);

            newFile.WriteLine(data);

            newFile.Flush();
        }

        public static void ExtractExceptions(Exception exception, ref List<Exception> exceptions)
        {
            exceptions.Add(exception);

            if (exception.InnerException != null)
            {
                ExtractExceptions(exception.InnerException, ref exceptions);
            }
        }

        public static ArgumentException CreateExceptionForInvaidEnumValue<T>(T value) where T : Enum
        {
            return new ArgumentException($"Enum {typeof(T).Name} does not have value like: {value}");
        }

        public static DateTime ConvertUnixTime(double milliseconds)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(milliseconds);
        }

        public static void IfNotNullSetValue<T>(ref T parameter, T value, string parameterName)
        {
            if (value is null)
            {
                throw new Exception($"{parameterName} value can not be null!");
            }

            parameter = value;
        }

        public static string GetPropertyValuesInString(object obj)
        {
            var summary = new StringBuilder();

            var objType = obj.GetType();

            var propetiesNamesAndValues = objType
                .GetProperties()
                .Select(property => $"{property.Name}: {property.GetValue(obj) ?? "null"}");

            summary.AppendJoin(", ", propetiesNamesAndValues);

            return $"<{objType.Name}>: {{{summary}}}";
        }

        public static TTo ConvertEnumToAnotherEnum<TFrom, TTo>(TFrom from)
            where TFrom : struct, Enum
            where TTo : struct, Enum
        {
            var isSuccessful = Enum.TryParse(Enum.GetName(from), true, out TTo result);

            if (!isSuccessful)
            {
                throw new Exception($"Can not convert value \"{from}\" to enum \"{typeof(TTo).Name}\"");
            }

            return result;
        }

    }
}
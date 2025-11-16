using System.Reflection;
using System.Web;

namespace CerbiSharp.Infrastructure.Base.Net
{
    public class QueryStringBuilder
    {
        public static Uri CreateQueryStringBuilder<T>(T obj, string url = "")
        {
            var builder = new UriBuilder(url)
            {
                Port = -1
            };

            List<string> queryParams = CreateQueryParams(obj);

            builder.Query = string.Join("&", queryParams);

            return new Uri(builder.ToString());
        }

        private static List<string> CreateQueryParams<T>(T obj)
        {
            PropertyInfo[] properties = GetPropeties(obj);

            var queryParams = new List<string>();

            foreach (var property in properties)
            {
                CheckIsValidTypeAndCreateQueryParamAndValue(obj, queryParams, property);
            }

            return queryParams;
        }

        private static PropertyInfo[] GetPropeties<T>(T obj)
        {
            return obj.GetType().GetProperties();
        }

        private static void CheckIsValidTypeAndCreateQueryParamAndValue<T>(T obj, List<string> queryParams, PropertyInfo property)
        {
            if (IsAbleToCreateQueryParam(property))
            {
                string value = GetValueOfTypeForQueryParam(obj, property);

                if (value != null)
                {
                    queryParams.Add($"{property.Name}={HttpUtility.UrlEncode(value)}");
                }
            }
        }

        private static bool IsAbleToCreateQueryParam(PropertyInfo property)
        {
            return
                property.PropertyType == typeof(decimal) ||
                property.PropertyType == typeof(double) ||
                property.PropertyType == typeof(int) ||
                property.PropertyType == typeof(string);
        }

        private static string GetValueOfTypeForQueryParam<T>(T obj, PropertyInfo property)
        {
            return property.GetValue(obj)?.ToString();
        }
    }
}

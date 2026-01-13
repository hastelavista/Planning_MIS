using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class Extension
    {
        public static bool IsValidMobileNo(this string value)
        {
            return (!string.IsNullOrEmpty(value) && value.StartsWith("9") && value.Length == 10);
        }
        public static string TransformToString(this List<int> values)
        {
            var val = string.Join(",", values);
            return val;
        }
        public static string TransformToString(this int[] values)
        {
            var val = string.Join(",", values);
            return val;
        }
        public static string TransformToString(this List<string> values)
        {
            var val = string.Join(",", values);
            return val;
        }
        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input.First().ToString().ToUpper() + input.Substring(1);
        }
        public static string RepeateString(this string s, int times)
        {
            string value = string.Empty;
            for (int i = 0; i < times; i++)
            {
                value += s;
            }
            return value;
        }
        public static bool IsImageFile(this string extension)
        {
            extension = extension.ToLower();
            return extension.Contains("jpg") || extension.Contains("jpeg") || extension.Contains("png") || extension.Contains("gif");
        }
        public static string GetBase64(this IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                string s = Convert.ToBase64String(fileBytes);

                return s;
            }
        }

    }

    public static class DataHelper
    {
        public static string ConvertToJSON(this object model)
        {
            if (model != null)
            {
                return JsonConvert.SerializeObject(model);
            }
            return null;
        }
        public static List<SqlParameter> AddMore(this List<SqlParameter> parameters, string name, object value, bool parseDbNull = true)
        {
            if (value == null)
                parameters.Add(new SqlParameter(name, DBNull.Value));
            else
                parameters.Add(new SqlParameter(name, value));
            return parameters;
        }

        public static List<SqlParameter> AddMore(this List<SqlParameter> parameters, string name, int? value)
        {
            var p = new SqlParameter(name, value);
            if (!value.HasValue)
                p.Value = DBNull.Value;
            parameters.Add(p);
            return parameters;
        }
    }



    public class DbResponse
    {
        public string Message { get; set; }
        public bool HasError { get; set; }
        public object Response { get; set; }
        public string Key { get; set; }
    }
    public class ApiResponse
    {
        public string Message { get; set; }
        public bool HasError { get; set; }
        public object Response { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

}

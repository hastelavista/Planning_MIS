using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class SQLHelper
    {
        public static List<SqlParameter> PrepareSQLParameters<T>(this T item)
        {
            var model = new List<SqlParameter>();
            var properties = typeof(T).GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                if (prop.GetCustomAttribute<NotMappedAttribute>() != null)
                    continue;

                var val = prop.GetValue(item);
                object sqlValue;

                if (val == null)
                {
                    sqlValue = DBNull.Value;
                }
                else if (val is System.Collections.IEnumerable enumerable && !(val is string))
                {
                    // Convert any IEnumerable (List<int>, int[], List<string>, etc.) to CSV string
                    var items = new List<string>();
                    foreach (var v in enumerable)
                        items.Add(v.ToString());
                    sqlValue = string.Join(",", items);
                }
                else
                {
                    sqlValue = val;
                }

                model.Add(new SqlParameter($"@{prop.Name}", sqlValue));
            }

            return model;
        }

        public static IList<T> TransformToList<T>(this DataTable dt) where T : class, new()
        {
            var result = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                var item = new T();
                foreach (var prop in typeof(T).GetProperties())
                {
                    if (!dt.Columns.Contains(prop.Name) || row[prop.Name] == DBNull.Value)
                        continue;

                    var value = row[prop.Name];
                    if (prop.PropertyType == typeof(List<int>) && value is string strVal)
                    {
                        try
                        {
                            var list = JsonConvert.DeserializeObject<List<int>>(strVal);
                            prop.SetValue(item, list);
                        }
                        catch
                        {
                            var list = strVal.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())).ToList();
                            prop.SetValue(item, list);
                        }
                    }
                    // Handle List<string>
                    else if (prop.PropertyType == typeof(List<string>) && value is string strVal2)
                    {
                        try
                        {
                            var list = JsonConvert.DeserializeObject<List<string>>(strVal2);
                            prop.SetValue(item, list);
                        }
                        catch
                        {
                            var list = strVal2.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                            prop.SetValue(item, list);
                        }
                    }
                    else
                    {
                        prop.SetValue(item, value);

                    }
                }

                result.Add(item);
            }

            return result;
        }

        public static T TransformToObject<T>(this DataTable dt) where T : class, new()
        {
            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            var item = new T();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (dt.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                    prop.SetValue(item, row[prop.Name]);
            }
            return item;
        }
    }
}

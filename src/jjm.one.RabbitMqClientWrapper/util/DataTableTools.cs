using jjm.one.RabbitMqClientWrapper.types;
using System;
using System.Collections.Generic;
using System.Data;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.util
{
    public static class DataTableTools
    {
        public static DataTable ToDataTable(this RmqcMessage message)
        {
            var res = new DataTable();

            res.Columns.Add("Property", typeof(string));
            res.Columns.Add("Value", typeof(string));

            var propertyInfos = message.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var type = propertyInfo.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = Nullable.GetUnderlyingType(type);
                }

                if (type != typeof(bool) &&
                    type != typeof(ulong) &&
                    type != typeof(string) &&
                    type != typeof(DateTime))
                {
                    continue;
                }

                var newRow = res.NewRow();
                newRow["Property"] = propertyInfo.Name;
                newRow["Value"] = propertyInfo.GetValue(message)?.ToString() ?? string.Empty;
                res.Rows.Add(newRow);
            }

            return res;
        }

        public static DataTable ToDataTable(this IBasicProperties basicProperties)
        {
            var res = new DataTable();

            res.Columns.Add("Property", typeof(string));
            res.Columns.Add("Value", typeof(string));

            var propertyInfos = basicProperties.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var type = propertyInfo.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = Nullable.GetUnderlyingType(type);
                }

                if (type == typeof(Dictionary<string,object>))
                {
                    continue;
                }

                var newRow = res.NewRow();
                newRow["Property"] = propertyInfo.Name;
                newRow["Value"] = propertyInfo.GetValue(basicProperties)?.ToString() ?? string.Empty;
                res.Rows.Add(newRow);
            }

            return res;
        }

        public static DataTable ToDataTable(this IDictionary<string, object> dict)
        {
            var res = new DataTable();

            res.Columns.Add("Property", typeof(string));
            res.Columns.Add("Value", typeof(string));

            foreach (var entry in dict)
            {
                var newRow = res.NewRow();
                newRow["Property"] = entry.Key;
                newRow["Value"] = entry.Value;
                res.Rows.Add(newRow);
            }

            return res;
        }
    }
}

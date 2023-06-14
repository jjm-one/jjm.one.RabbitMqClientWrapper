using jjm.one.RabbitMqClientWrapper.types;
using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.util
{
    public static class DataTableTools
    {
        public static RmqcPropertyValueDataTable ToDataTable(this RmqcMessage message)
        {
            var res = new RmqcPropertyValueDataTable();

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
                newRow.Property = propertyInfo.Name;
                newRow.Value = propertyInfo.GetValue(message).ToString();

                res.Add(newRow);
            }

            return res;
        }

        public static RmqcPropertyValueDataTable ToDataTable(this IBasicProperties basicProperties)
        {
            var res = new RmqcPropertyValueDataTable();

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
                newRow.Property = propertyInfo.Name;
                newRow.Value = propertyInfo.GetValue(basicProperties).ToString();

                res.Add(newRow);
            }

            return res;
        }

        public static RmqcPropertyValueDataTable ToDataTable(this IDictionary<string, object> dict)
        {
            var res = new RmqcPropertyValueDataTable();

            foreach (var entry in dict)
            {
                var newRow = res.NewRow();
                newRow.Property = entry.Key;
                newRow.Value = entry.Value.ToString();

                res.Add(newRow);
            }

            return res;
        }
    }
}

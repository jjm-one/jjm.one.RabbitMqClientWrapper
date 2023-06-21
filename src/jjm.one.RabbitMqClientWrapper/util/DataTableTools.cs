using jjm.one.RabbitMqClientWrapper.types;
using System;
using System.Collections.Generic;
using System.Data;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.util
{
    public static class DataTableTools
    {
        #region public extension functions

        public static DataTable ToDataTable(this RmqcMessage message)
        {
            var res = new DataTable();

            res.Columns.Add("Property", typeof(string));
            res.Columns.Add("Value", typeof(object));
            res.Columns.Add("ReadOnly", typeof(bool));
            res.Columns.Add("Type", typeof(Type));

            var propertyInfos = message.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var type = propertyInfo.PropertyType;
                var ogType = type;

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
                newRow["Value"] = propertyInfo.GetValue(message);
                newRow["ReadOnly"] = propertyInfos.IsReadOnly;
                newRow["Type"] = ogType;
                res.Rows.Add(newRow);
            }

            return res;
        }

        public static DataTable ToDataTable(this IBasicProperties basicProperties)
        {
            var res = new DataTable();

            res.Columns.Add("Property", typeof(string));
            res.Columns.Add("Value", typeof(string));
            res.Columns.Add("Type", typeof(Type));

            var propertyInfos = basicProperties.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var type = propertyInfo.PropertyType;
                var ogType = type;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = Nullable.GetUnderlyingType(type);
                }

                if (type == typeof(IDictionary<string, object>))
                {
                    continue;
                }

                var newRow = res.NewRow();
                newRow["Property"] = propertyInfo.Name;
                newRow["Value"] = propertyInfo.GetValue(basicProperties);
                newRow["Type"] = ogType;
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

        #endregion

        #region public functions

        public static DataTablesToMessageConversionStatus DataTablesToMessage(ref RmqcMessage? message,
            DataTable metadata, DataTable basicProperties, DataTable headers)
        {
            var res = DataTablesToMessageConversionStatus.None;

            // check input --------------------------------------------------------
            
            if (message is null)
            {
                res |= DataTablesToMessageConversionStatus.OgMsgNull;
                message = new RmqcMessage();
            }

            if (message.BasicProperties is null)
            {
                res |= DataTablesToMessageConversionStatus.OgBasicPropNull;
            }

            if (message.Headers is null)
            {
                res |= DataTablesToMessageConversionStatus.OgHeadersNull;
                message.Headers = new Dictionary<string, object>();
            }

            // check input end ----------------------------------------------------

            // fill data ----------------------------------------------------------

            if (!UpdateMetaDataInMessage(ref message, metadata))
            {
                res |= DataTablesToMessageConversionStatus.Error;
                res |= DataTablesToMessageConversionStatus.MissformedMetaData;
            }

            if (message.BasicProperties is not null && !UpdateBasicPropsInMessage(ref message, basicProperties))
            {
                res |= DataTablesToMessageConversionStatus.Warning;
                res |= DataTablesToMessageConversionStatus.MissformedBasicProps;
            }

            if (!UpdateHeadersInMessage(ref message, headers))
            {
                res |= DataTablesToMessageConversionStatus.Error;
                res |= DataTablesToMessageConversionStatus.MissformedHeaders;
            }
            
            // fill data end ------------------------------------------------------

            return res;
        }

        #endregion

        #region public enunms

        [Flags]
        public enum DataTablesToMessageConversionStatus : ushort
        {
            None = 0,
            Ok = 1 << 0,
            OgMsgNull = 1 << 1,
            OgBasicPropNull = 1 << 2,
            OgHeadersNull = 1 << 3,
            Error = 1 << 4,
            Warning = 1 << 5,
            MissformedMetaData = 1 << 6,
            MissformedBasicProps = 1 << 7,
            MissformedHeaders = 1 << 8,

        }

        public static bool Failed(this DataTablesToMessageConversionStatus state)
        {
            return (state & DataTablesToMessageConversionStatus.Error) > 0 ||
                   (state & DataTablesToMessageConversionStatus.MissformedMetaData) > 0 ||
                   (state & DataTablesToMessageConversionStatus.MissformedBasicProps) > 0 ||
                   (state & DataTablesToMessageConversionStatus.MissformedHeaders) > 0;
        }

        #endregion

        #region private functions

        private static bool UpdateMetaDataInMessage(ref RmqcMessage message, DataTable metadata)
        {
            // data table check
            if (!metadata.Columns.Contains("Property") ||
                !metadata.Columns.Contains("Value") ||
                !metadata.Columns.Contains("ReadOnly") ||
                !metadata.Columns.Contains("Type"))
            {
                return false;
            }

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

                foreach (DataRow row in metadata.Rows)
                {
                    if (row["Property"].Equals(propertyInfo.Name))
                    {
                        propertyInfo.SetValue(message, row["Value"]);
                    }
                }
            }

            return true;
        }

        private static bool UpdateBasicPropsInMessage(ref RmqcMessage message, DataTable basicProperties)
        {
            // data table check
            if (!basicProperties.Columns.Contains("Property") ||
                !basicProperties.Columns.Contains("Value") ||
                !basicProperties.Columns.Contains("ReadOnly") ||
                !basicProperties.Columns.Contains("Type"))
            {
                return false;
            }

            var propertyInfos = message.BasicProperties?.GetType().GetProperties();
            if (propertyInfos is null)
            {
                return false;
            }
            foreach (var propertyInfo in propertyInfos)
            {
                var type = propertyInfo.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = Nullable.GetUnderlyingType(type);
                }

                if (type == typeof(IDictionary<string, object>))
                {
                    continue;
                }

                foreach (DataRow row in basicProperties.Rows)
                {
                    if (row["Property"].Equals(propertyInfo.Name))
                    {
                        propertyInfo.SetValue(message.BasicProperties, row["Value"]);
                    }
                }
            }

            return true;
        }

        private static bool UpdateHeadersInMessage(ref RmqcMessage message, DataTable headers)
        {
            // data table check
            if (!headers.Columns.Contains("Property") ||
                !headers.Columns.Contains("Value"))
            {
                return false;
            }

            if (message.Headers is null)
            {
                return false;
            }
            foreach (DataRow row in headers.Rows)
            {
                message.Headers.Add(row["Property"].ToString(), row["Value"]);
            }

            return true;
        }

        #endregion
    }
}

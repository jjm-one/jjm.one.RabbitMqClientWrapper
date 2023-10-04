using jjm.one.RabbitMqClientWrapper.types;
using System;
using System.Collections.Generic;
using System.Data;
using RabbitMQ.Client;
using System.Text;

namespace jjm.one.RabbitMqClientWrapper.util;

/// <summary>
/// This static class contains functions and extensions to convert a <see cref="RmqcMessage"/> and it's elements to a <see cref="DataTable"/> and wise versa.
/// </summary>
public static class DataTableTools
{
    #region public extension functions

    /// <summary>
    /// This extension converts a <see cref="RmqcMessage"/> into a <see cref="DataTable"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
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
            newRow["ReadOnly"] = !propertyInfo.CanWrite;
            newRow["Type"] = ogType;
            res.Rows.Add(newRow);
        }

        return res;
    }

    /// <summary>
    /// This extension converts a <see cref="IBasicProperties"/> into a <see cref="DataTable"/>.
    /// </summary>
    /// <param name="basicProperties"></param>
    /// <returns></returns>
    public static DataTable ToDataTable(this IBasicProperties basicProperties)
    {
        var res = new DataTable();

        res.Columns.Add("Property", typeof(string));
        res.Columns.Add("Value", typeof(string));
        res.Columns.Add("ReadOnly", typeof(bool));
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
            newRow["ReadOnly"] = !propertyInfo.CanWrite;
            newRow["Type"] = ogType;
            res.Rows.Add(newRow);
        }

        return res;
    }

    /// <summary>
    /// This extension converts a <see cref="IDictionary{TKey,TValue}"/> into a <see cref="DataTable"/>.
    /// </summary>
    /// <param name="dict"></param>
    /// <returns></returns>
    public static DataTable ToDataTable(this IDictionary<string, object> dict)
    {
        var res = new DataTable();

        res.Columns.Add("Property", typeof(string));
        res.Columns.Add("Value", typeof(string));
        res.Columns.Add("Type", typeof(Type));

        foreach (var entry in dict)
        {
            var newRow = res.NewRow();
            newRow["Property"] = entry.Key;

            switch (entry.Value)
            {
                case bool b:
                    newRow["Value"] = b;
                    newRow["Type"] = typeof(bool);
                    break;
                case long l:
                    newRow["Value"] = l;
                    newRow["Type"] = typeof(long);
                    break;
                case byte[] bytes:
                    newRow["Value"] = Encoding.UTF8.GetString(bytes);
                    newRow["Type"] = typeof(string);
                    break;
                default:
                    newRow["Value"] = "NOT SUPPORTED TYPE!";
                    newRow["Type"] = entry.Value.GetType();
                    break;
            }

            res.Rows.Add(newRow);
        }

        return res;
    }

    #endregion

    #region public functions

    /// <summary>
    /// This extension converts a <see cref="DataTable"/> into a <see cref="RmqcMessage"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="metadata"></param>
    /// <param name="basicProperties"></param>
    /// <param name="headers"></param>
    /// <returns></returns>
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
            res |= DataTablesToMessageConversionStatus.MissFormattedMetaData;
        }

        if (message.BasicProperties is not null && !UpdateBasicPropsInMessage(ref message, basicProperties))
        {
            res |= DataTablesToMessageConversionStatus.Warning;
            res |= DataTablesToMessageConversionStatus.MissFormattedBasicProps;
        }

        if (!UpdateHeadersInMessage(ref message, headers))
        {
            res |= DataTablesToMessageConversionStatus.Error;
            res |= DataTablesToMessageConversionStatus.MissFormattedHeaders;
        }
            
        // fill data end ------------------------------------------------------

        return res;
    }

    #endregion

    #region public enunms

    /// <summary>
    /// This enum represents the conversion status when converting <see cref="DataTable"/> to <see cref="RmqcMessage"/>.
    /// </summary>
    [Flags]
    public enum DataTablesToMessageConversionStatus : ushort
    {
        /// <summary>
        /// Status = None
        /// </summary>
        None = 0,
        /// <summary>
        /// Status = ok
        /// </summary>
        Ok = 1 << 0,
        /// <summary>
        /// Status = origin message is null
        /// </summary>
        OgMsgNull = 1 << 1,
        /// <summary>
        /// Status = origin basic properties are null
        /// </summary>
        OgBasicPropNull = 1 << 2,
        /// <summary>
        /// Status = origin headers are null
        /// </summary>
        OgHeadersNull = 1 << 3,
        /// <summary>
        /// Status = error
        /// </summary>
        Error = 1 << 4,
        /// <summary>
        /// status = warning
        /// </summary>
        Warning = 1 << 5,
        /// <summary>
        /// Status = miss formatted meta data
        /// </summary>
        MissFormattedMetaData = 1 << 6,
        /// <summary>
        /// Status = miss formatted basic properties
        /// </summary>
        MissFormattedBasicProps = 1 << 7,
        /// <summary>
        /// Status = miss formatted headers
        /// </summary>
        MissFormattedHeaders = 1 << 8,

    }

    /// <summary>
    /// This function checks whether the conversion failed or not based on the <see cref="DataTablesToMessageConversionStatus"/>.
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static bool Failed(this DataTablesToMessageConversionStatus state)
    {
        return (state & DataTablesToMessageConversionStatus.Error) > 0 ||
               (state & DataTablesToMessageConversionStatus.MissFormattedMetaData) > 0 ||
               (state & DataTablesToMessageConversionStatus.MissFormattedBasicProps) > 0 ||
               (state & DataTablesToMessageConversionStatus.MissFormattedHeaders) > 0;
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
                    if (propertyInfo.CanWrite)
                    {
                        if (type != row["Value"].GetType() && row["Value"] is string)
                        {
                            if (type == typeof(bool))
                            {
                                var res = bool.TryParse((string)row["Value"], out var value);
                                if (res) propertyInfo.SetValue(message, value);
                            }
                            else if (type == typeof(ulong))
                            {
                                var res = ulong.TryParse((string)row["Value"], out var value);
                                if (res) propertyInfo.SetValue(message, value);
                            }
                        }
                        else {
                            propertyInfo.SetValue(message, row["Value"] is DBNull ? null : row["Value"]);
                        }
                    }
                    break;
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

            if (type != typeof(bool) &&
                type != typeof(ushort) &&
                type != typeof(string) &&
                type != typeof(AmqpTimestamp))
            {
                continue;
            }

            foreach (DataRow row in basicProperties.Rows)
            {
                if (row["Property"].Equals(propertyInfo.Name))
                {
                    if (propertyInfo.CanWrite)
                    {
                        if (type != row["Value"].GetType() && row["Value"] is string)
                        {
                            if (type == typeof(bool))
                            {
                                var res = bool.TryParse((string)row["Value"], out var value);
                                if (res) propertyInfo.SetValue(message.BasicProperties, value);
                            }
                            else if (type == typeof(ushort))
                            {
                                var res = ushort.TryParse((string)row["Value"], out var value);
                                if (res) propertyInfo.SetValue(message.BasicProperties, value);
                            }
                        }
                        else
                        {
                            propertyInfo.SetValue(message.BasicProperties, row["Value"] is DBNull ? null : row["Value"]);
                        }
                    }
                    break;
                }
            }
        }

        return true;
    }

    private static bool UpdateHeadersInMessage(ref RmqcMessage message, DataTable headers)
    {
        // data table check
        if (!headers.Columns.Contains("Property") ||
            !headers.Columns.Contains("Value") ||
            !headers.Columns.Contains("Type") )
        {
            return false;
        }

        if (message.Headers is null)
        {
            return false;
        }
        foreach (DataRow row in headers.Rows)
        {
            var p = row["Property"].ToString();
            var v = row["Value"];
            if (string.IsNullOrEmpty(p) || v is DBNull)
            {
                continue;
            }

            message.Headers[p] = v;
        }

        return true;
    }

    #endregion
}
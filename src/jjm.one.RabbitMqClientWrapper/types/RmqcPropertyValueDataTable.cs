using System;
using System.Data;

namespace jjm.one.RabbitMqClientWrapper.types
{
    /// <summary>
    /// see https://www.codeproject.com/Articles/30490/How-to-Manually-Create-a-Typed-DataTable
    /// </summary>
    public class RmqcPropertyValueDataTable : DataTable
    {
        #region ctor

        public RmqcPropertyValueDataTable()
        {
            Columns.Add("Property", typeof(string));
            Columns.Add("Value", typeof(string));
        }

        #endregion

        protected override Type GetRowType()
        {
            return typeof(RmqcPropertyValueDataTableRow);
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new RmqcPropertyValueDataTableRow(builder);
        }

        // indexer
        public RmqcPropertyValueDataTableRow this[int idx] => (RmqcPropertyValueDataTableRow)Rows[idx];

        public void Add(RmqcPropertyValueDataTableRow row)
        {
            Rows.Add(row);
        }

        public new RmqcPropertyValueDataTableRow NewRow()
        {
            var row = NewRow();

            return row;
        }
    }

    public class RmqcPropertyValueDataTableRow : DataRow
    {
        public string Property
        {
            get => (string)base["Property"];
            set => base["Property"] = value;
        }

        public string Value
        {
            get => (string)base["Value"];
            set => base["Value"] = value;
        }

        protected internal RmqcPropertyValueDataTableRow(DataRowBuilder builder) : base(builder)
        {
            Property = string.Empty;
            Value = string.Empty;
        }

        
    }
}

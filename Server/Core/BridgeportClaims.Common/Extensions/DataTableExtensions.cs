using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using BridgeportClaims.Common.Helpers;

namespace BridgeportClaims.Common.Extensions
{
    public static class DataTableExtensions
    {
        public static DataTable ToDynamicLinqDataTable(this IQueryable items)
        {
            var type = items.ElementType;

            // Create the result table, and gather all properties of a type.
            var table = new DataTable(type.Name);

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Add the properties as columns to the datatable
            foreach (var prop in props)
            {
                var propType = prop.PropertyType;

                // Is it a nullable type? Get the underlying type 
                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propType = Nullable.GetUnderlyingType(propType);
                }

                if (null != propType)
                    table.Columns.Add(prop.Name, propType);
            }

            // Add the property values as rows to the datatable
            foreach (var item in items)
            {
                var values = new object[props.Length];

                if (item != null)
                {
                    for (var i = 0; i < props.Length; i++)
                    {
                        values[i] = props[i].GetValue(item, null);
                    }
                }

                table.Rows.Add(values);
            }

            return table;
        }

        /// <summary>
        /// SetOrdinal of DataTable columns based on the index of the columnNames array. Removes invalid column names first.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnNames"></param>
        /// <remarks> http://stackoverflow.com/questions/3757997/how-to-change-datatable-colums-order </remarks>
        public static void SetColumnsOrder(this DataTable dt, params string[] columnNames)
        {
            if (null == columnNames)
                throw new ArgumentNullException(nameof(columnNames));

            var listColNames = columnNames.ToList();

            //Remove invalid column names.
            foreach (var colName in columnNames)
                if (!dt.Columns.Contains(colName))
                    listColNames.Remove(colName);

            foreach (var colName in listColNames)
                dt.Columns[colName].SetOrdinal(listColNames.IndexOf(colName));
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            var dt = new DataTable();
            var t = typeof(T);
            var pia = t.GetProperties();
            //Create the columns in the DataTable
            foreach (var pi in pia)
            {
                dt.Columns.Add(pi.Name, pi.PropertyType);
            }

            //Populate the table
            foreach (var item in collection)
            {
                var dr = dt.NewRow();
                dr.BeginEdit();
                foreach (var pi in pia)
                {
                    dr[pi.Name] = pi.GetValue(item, null);
                }

                dr.EndEdit();
                dt.Rows.Add(dr);
            }

            return dt;

        }


        public static DataTable ToFixedDataTable<T>(this IEnumerable<T> values) where T : class, new()
        {
            var table = new DataTable();
            var enumerable = values as T[] ?? values.ToArray();
            var members = enumerable.First().GetType().GetProperties();
            foreach (var member in members)
                table.Columns.Add(member.Name, Nullable.GetUnderlyingType(member.PropertyType) ?? member.PropertyType);
            foreach (var value in enumerable)
            {
                var row = table.NewRow();
                row.ItemArray = members.Select(s => s.GetValue(value) ?? DBNull.Value).ToArray();
                table.Rows.Add(row);
            }

            return table;
        }

        public static DataTable ToShreddedDataTable<T>(this IEnumerable<T> source) =>
            new ObjectShredder<T>().Shred(source, null, null);

        public static DataTable CopyToGenericDataTable<T>(this IEnumerable<T> source,
            DataTable table, LoadOption? options) => new ObjectShredder<T>().Shred(source, table, options);
    }
}
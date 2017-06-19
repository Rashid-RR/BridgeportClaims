using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BridgeportClaims.Common.Extensions
{
    /// <summary>
    /// Linq Extensions
    /// </summary>
    public static class LinqExtensions
    {

        /// <summary>
        /// Converts the Linq data to a comma-separated string including header.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string ToCsvString(this IOrderedQueryable data)
        {
            return ToCsvString(data, "; ");
        }

        /// <summary>
        /// Converts the Linq data to a comma-separated string including header.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static string ToCsvString(this IOrderedQueryable data, string delimiter)
        {
            return ToCsvString(data, "; ", null);
        }

        /// <summary>
        /// Converts the Linq data to a comma-separated string including header.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="nullvalue">The nullvalue.</param>
        /// <returns></returns>
        public static string ToCsvString(this IOrderedQueryable data, string delimiter, string nullvalue)
        {
            var csvdata = new StringBuilder();
            var replaceFrom = delimiter.Trim();
            var replaceDelimiter = ";";
            var headers = data.ElementType.GetProperties();
            switch (replaceFrom)
            {
                case ";":
                    replaceDelimiter = ":";
                    break;
                case ",":
                    replaceDelimiter = "¸";
                    break;
                case "\t":
                    replaceDelimiter = "    ";
                    break;
            }
            if (headers.Length > 0)
            {
                foreach (var head in headers)
                {
                    csvdata.Append(head.Name.Replace("_", " ") + delimiter);
                }
                csvdata.Append("\n");
            }
            foreach (var row in data)
            {
                var fields = row.GetType().GetProperties();
                foreach (var t in fields)
                {
                    var value = t.GetValue(row, null);
                    if (value != null)
                    {
                        csvdata.Append(value.ToString().Replace("\r", "\f").Replace("\n", " \f").Replace("_", " ").Replace(replaceFrom, replaceDelimiter) + delimiter);
                    }
                    else
                    {
                        csvdata.Append(nullvalue);
                        csvdata.Append(delimiter);
                    }
                }
                csvdata.Append("\n");
            }
            return csvdata.ToString();
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            var properties = typeof(T).GetProperties();

            var output = new DataTable();

            foreach (var prop in properties)
            {
                output.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in source)
            {
                var row = output.NewRow();

                foreach (var prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item, null);
                }

                output.Rows.Add(row);
            }

            return output;
        }
    }
}

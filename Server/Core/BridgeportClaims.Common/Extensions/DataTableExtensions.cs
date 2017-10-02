using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BridgeportClaims.Common.Extensions
{
    public static class DataTableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> values) where T : class, new()
        {
            var table = new DataTable();
            var enumerable = values as T[] ?? values.ToArray();
            var members = enumerable.First().GetType().GetProperties();
            foreach (var member in members)
                table.Columns.Add(member.Name, member.PropertyType);
            foreach (var value in enumerable)
            {
                var row = table.NewRow();
                row.ItemArray = members.Select(s => s.GetValue(value)).ToArray();
                table.Rows.Add(row);
            }
            return table;
        }
    }
}

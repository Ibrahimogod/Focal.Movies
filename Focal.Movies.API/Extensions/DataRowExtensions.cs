using System.Data;
using System.Reflection;
using CsvHelper.Configuration.Attributes;

namespace Focal.Movies.API.Extensions;

public static class DataRowExtensions
{
    public static List<T> ToList<T>(this IEnumerable<DataRow> dataRows) where T : class, new()
    {
        var properties = typeof(T).GetProperties();
        var list = new List<T>();

        foreach (DataRow row in dataRows)
        {
            var obj = new T();
            foreach (var prop in properties)
            {
                var nameAttribute = prop.GetCustomAttribute<NameAttribute>();
                string columnName = prop.Name;
                if (nameAttribute != null)
                {
                    columnName = nameAttribute.Names.First();
                }

                if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
                {
                    prop.SetValue(obj, Convert.ChangeType(row[columnName], prop.PropertyType));
                }
            }

            list.Add(obj);
        }

        return list;
    }
}
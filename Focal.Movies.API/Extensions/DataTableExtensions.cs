using System.Data;
using System.Reflection;
using CsvHelper.Configuration.Attributes;

namespace Focal.Movies.API.Extensions;

public static class DataTableExtensions
{
    public static void AddRow<T>(this DataTable dataTable, T obj)
    {
        DataRow row = dataTable.NewRow();

        PropertyInfo[] properties = typeof(T).GetProperties();

        foreach (PropertyInfo property in properties)
        {
            var nameAttribute = property.GetCustomAttribute<NameAttribute>();
            string columnName = property.Name;
            if (nameAttribute != null)
            {
                columnName = nameAttribute.Names.First();
            }
            if (dataTable.Columns.Contains(columnName))
            {
                if (row[columnName] == DBNull.Value)
                {
                    row[columnName] = property.GetValue(obj);
                }
            }
        }

        dataTable.Rows.Add(row);
    }
}
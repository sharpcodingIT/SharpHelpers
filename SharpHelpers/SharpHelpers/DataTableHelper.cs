// (c) 2020 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SharpCoding.SharpHelpers
{
    public static class DataTableHelper
    {
        /// <summary>
        /// This method set the columns order by name
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        public static DataTable SetColumnsOrder(this DataTable table, string[] columnNames)
        {
            int columnIndex = 0;
            foreach (var columnName in columnNames)
            {
                if (table.Columns.Contains(columnName))
                {
                    table.Columns[columnName].SetOrdinal(columnIndex);
                    columnIndex++;
                }
            }
            return table;
        }

        /// <summary>
        /// This method return a list of objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            var list = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                var objClass = new T();
                var type = objClass.GetType();
                foreach (DataColumn column in row.Table.Columns)
                {
                    var prop = type.GetProperty(column.ColumnName);
                    if (prop != null)
                        prop.SetValue(objClass, row[column.ColumnName], null);
                }
                list.Add(objClass);
            }
            return list;
        }

        /// <summary>
        /// Converts the DataTable to a CSV format string.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string ToCsv(this DataTable table, string delimiter = ",")
        {
            if (table == null) throw new ArgumentNullException(nameof(table));

            var csv = new List<string>();
            var headers = string.Join(delimiter, table.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            csv.Add(headers);

            foreach (DataRow row in table.Rows)
            {
                var line = string.Join(delimiter, row.ItemArray.Select(field => field?.ToString()));
                csv.Add(line);
            }
            return string.Join(Environment.NewLine, csv);
        }

        /// <summary>
        /// Adds a new column to the DataTable with the specified default value.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnName"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        public static void AddColumn<T>(this DataTable table, string columnName, T defaultValue = default)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));

            var column = new DataColumn(columnName, typeof(T)) { DefaultValue = defaultValue };
            table.Columns.Add(column);
            foreach (DataRow row in table.Rows)
            {
                row[columnName] = defaultValue;
            }
        }

        /// <summary>
        /// Merges multiple DataTables with the same schema into one.
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static DataTable MergeTables(IEnumerable<DataTable> tables)
        {
            if (tables == null) throw new ArgumentNullException(nameof(tables));

            var resultTable = tables.First().Clone();
            foreach (var table in tables)
            {
                if (!AreSchemasCompatible(resultTable, table))
                    throw new ArgumentException("Tables have incompatible schemas.");

                foreach (DataRow row in table.Rows)
                {
                    resultTable.ImportRow(row);
                }
            }
            return resultTable;
        }

        private static bool AreSchemasCompatible(DataTable table1, DataTable table2)
        {
            if (table1.Columns.Count != table2.Columns.Count) return false;

            for (int i = 0; i < table1.Columns.Count; i++)
            {
                if (table1.Columns[i].ColumnName != table2.Columns[i].ColumnName ||
                    table1.Columns[i].DataType != table2.Columns[i].DataType)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Filters the rows in the DataTable based on a predicate.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static DataTable Filter(this DataTable table, Func<DataRow, bool> predicate)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var filteredTable = table.Clone();
            foreach (DataRow row in table.AsEnumerable().Where(predicate))
            {
                filteredTable.ImportRow(row);
            }
            return filteredTable;
        }

        /// <summary>
        /// Checks if the DataTable is empty (contains no rows).
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool IsEmpty(this DataTable table)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));

            return table.Rows.Count == 0;
        }

        /// <summary>
        /// Removes duplicate rows based on specified columns.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        public static DataTable RemoveDuplicates(this DataTable table, params string[] columnNames)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));

            var distinctTable = table.Clone();
            var uniqueRows = new HashSet<string>();

            foreach (DataRow row in table.Rows)
            {
                var key = string.Join("|", columnNames.Select(c => row[c]?.ToString() ?? ""));
                if (uniqueRows.Add(key))
                {
                    distinctTable.ImportRow(row);
                }
            }
            return distinctTable;
        }
    }
}

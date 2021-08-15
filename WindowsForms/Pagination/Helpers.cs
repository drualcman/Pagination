using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms.Pagination
{
    class Helpers
    {
        /// <summary>
        /// Convert DataTable to IEnumerable
        /// </summary>
        /// <param name="table">DataTable to Convert</param>
        /// <returns></returns>
        public static IEnumerable<DataRow> AsEnumerable(DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                yield return table.Rows[i];
            }
        }

        /// <summary>
        /// Convert DataTable to IEnumerable
        /// </summary>
        /// <param name="table">DataTable to Convert</param>
        /// <returns></returns>
        public static IQueryable<DataRow> AsQuerable(DataTable table)
        {
            return AsEnumerable(table).AsQueryable();
        }
    }
}

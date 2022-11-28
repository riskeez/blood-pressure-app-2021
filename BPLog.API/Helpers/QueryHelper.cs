using BPLog.API.Services;
using System;

namespace BPLog.API.Helpers
{
    /// <summary>
    /// Helpers to simplify some query parsing work
    /// </summary>
    public static class QueryHelper
    {
        /// <summary>
        /// Gets Sort order from provided string
        /// </summary>
        /// <param name="valueWithSort">Value with sort condition (can have separate value or have it in the end)</param>
        /// <returns>SortOrder (Descending by default)</returns>
        public static SortOrder GetParamSortOrder(string valueWithSort)
        {
            SortOrder sorting = SortOrder.Descending;
            if (!string.IsNullOrWhiteSpace(valueWithSort))
            {
                if (valueWithSort.Trim().Equals("asc", StringComparison.OrdinalIgnoreCase) || valueWithSort.TrimEnd().EndsWith(" asc", StringComparison.OrdinalIgnoreCase))
                {
                    sorting = SortOrder.Ascending;
                }
            }
            return sorting;
        }
    }
}

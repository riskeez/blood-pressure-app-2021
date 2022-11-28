using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPLog.API.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Take<T>(this IQueryable<T> query, int start, int count)
        {
            IQueryable<T> resQuery = start > 0 ? query.Skip(start) : query;
            return count > -1 ? resQuery.Take(count) : resQuery;
        }
    }
}

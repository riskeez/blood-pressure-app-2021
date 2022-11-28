using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPLog.API.Model
{
    /// <summary>
    /// Abstract page model class that helps when pagination is needed
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class PageResponse<TModel>
        where TModel : class, new()
    {
        /// <summary>
        /// Amount of pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Page data
        /// </summary>
        public IEnumerable<TModel> Payload { get; set; }
    }
}

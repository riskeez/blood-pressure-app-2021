using System;
using System.Collections.Generic;
using System.Text;

namespace BPLog.App.Models
{
    public abstract class BasePageResponse<TModel>
        where TModel : class, new()
    {
        public int TotalPages { get; set; }

        public IEnumerable<TModel> Payload { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BPLog.App.Models
{
    public class RestResponse
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class RestResponse<T> : RestResponse
    {
        public T Data { get; set; }
        
        public static RestResponse<T> OkResult(T data)
        {
            return new RestResponse<T>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = data
            };
        }

        public static RestResponse<T> ErrorResult(HttpStatusCode errorCode)
        {
            return new RestResponse<T>
            {
                StatusCode = errorCode
            };
        }
    }
}

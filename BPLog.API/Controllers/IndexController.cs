using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BPLog.API.Controllers
{
    /// <summary>
    /// Default controller
    /// </summary>
    [Route("[controller]")]
    public class IndexController : ControllerBase
    {
        /// <summary>
        /// Returns current version of the API
        /// </summary>
        /// <returns>Version info</returns>
        [HttpGet]
        [AllowAnonymous]
        public string Index()
        {
            string version = typeof(IndexController).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            return "BPLog API v" + version;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BPLog.API.Model
{
    /// <summary>
    /// User credentials model
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// User login
        /// </summary>
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}

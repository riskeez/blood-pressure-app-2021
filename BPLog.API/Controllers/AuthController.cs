using BPLog.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BPLog.API.Model;

namespace BPLog.API.Controllers
{
    /// <summary>
    /// List of endpoints for user authentication and registration
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserManager _manager;

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="manager">User Manager service</param>
        public AuthController(IUserManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Authorizes user and generates token that can be used to access API
        /// </summary>
        /// <param name="request">Login credentials</param>
        /// <returns>JWT access token</returns>
        [HttpPost()]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Login(LoginRequest request)
        {
            var user = await _manager.GetUserByLogin(request.Login);
            if (user != null)
            {
                string token = _manager.Authenticate(user, request.Password);
                if (!string.IsNullOrEmpty(token))
                {
                    return Ok(token);
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Registers a new user. Login must be unique
        /// </summary>
        /// <param name="request">New user credentials</param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser(LoginRequest request)
        {
            var user = await _manager.RegisterUser(request.Login, request.Password);
            if (user != null)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}

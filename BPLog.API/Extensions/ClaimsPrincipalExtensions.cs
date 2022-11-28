using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BPLog.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Gets user ID from principal claims
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static int? GetUserId(this ClaimsPrincipal principal)
        {
            var idClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (int.TryParse(idClaim?.Value, out int userID))
            {
                return userID;
            }
            return null;
        }
    }
}

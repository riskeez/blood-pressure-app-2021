using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace BPLog.App.Services
{
    public interface ITokenValidator
    {
        DateTime GetExpiryDate(string accessToken);
        bool IsValid(string accessToken);
    }

    public class TokenValidator : ITokenValidator
    {
        public DateTime GetExpiryDate(string accessToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(accessToken);
                return jsonToken.ValidTo;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public bool IsValid(string accessToken)
        {
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                var expiryDate = GetExpiryDate(accessToken);
                return DateTime.UtcNow < expiryDate;
            }
            return false;
        }
    }
}

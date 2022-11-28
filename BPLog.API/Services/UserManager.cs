using BPLog.API.Domain;
using BPLog.API.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BPLog.API.Services
{
    /// <summary>
    /// UserManager class that operates with database and generate JWTs
    /// </summary>
    public class UserManager : IUserManager
    {
        private readonly BPLogDbContext _dbContext;
        private readonly string _privateKey;
        private readonly ILogger<UserManager> _logger;

        public UserManager(BPLogDbContext dbContext, IConfiguration configuration, ILogger<UserManager> logger)
        {
            _dbContext = dbContext;
            _logger = logger;

            _privateKey = configuration.GetPrivateKey() ?? throw new ArgumentException("PrivateKey is missing. Check appsettings");
        }

        public async Task<User> GetUserByLogin(string login, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(login))
            {
                string userLogin = login.Trim().ToLowerInvariant();

                return await _dbContext.Users.FirstOrDefaultAsync(x => x.Login.Equals(userLogin), cancellationToken);
            }
            return null;
        }

        public async Task<User> RegisterUser(string login, string password, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            string newLogin = login.Trim().ToLowerInvariant();

            var existingUser = await GetUserByLogin(newLogin, cancellationToken);
            if (existingUser != null)
            {
                _logger.LogWarning("User already exists: ", newLogin);
                return null; 
            }

            var newUser = new User { Login = newLogin, PasswordHash = password.ToSha256() };
            _dbContext.Users.Add(newUser);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return newUser;
        }

        public string Authenticate(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password) || user == null)
            {
                return null;
            }

            if (!user.PasswordHash.Equals(password.ToSha256()))
            {
                _logger.LogWarning("Wrong password for user {user}", user.Login);
                return null;
            }

            // JWT generation
            byte[] key = Encoding.UTF8.GetBytes(_privateKey);
            var descriptor = new SecurityTokenDescriptor()
            {
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
            };

            var jwtHandler = new JwtSecurityTokenHandler();
            SecurityToken token = jwtHandler.CreateToken(descriptor);
            return jwtHandler.WriteToken(token);
        }

    }
}

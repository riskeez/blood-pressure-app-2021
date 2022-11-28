using BPLog.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BPLog.API.Services
{
    /// <summary>
    /// User management service for auth/user registration
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Gets user entity by provided login
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="cancellationToken"></param>
        /// <returns>User or null if no matches found</returns>
        Task<User> GetUserByLogin(string login, CancellationToken cancellationToken = default);

        /// <summary>
        /// Registers new user with provided credentials
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>User or null if registration failed</returns>
        Task<User> RegisterUser(string login, string password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Authenticates user and generates JWT token
        /// </summary>
        /// <param name="user">User entity</param>
        /// <param name="password">User plain password</param>
        /// <returns>JWT token or null if auth failed</returns>
        string Authenticate(User user, string password);
    }
}

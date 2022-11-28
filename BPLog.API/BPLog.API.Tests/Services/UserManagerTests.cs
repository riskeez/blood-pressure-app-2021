using Xunit;
using FluentAssertions;
using BPLog.API.Services;
using Microsoft.EntityFrameworkCore;
using BPLog.API.Domain;
using System.Collections.Generic;
using BPLog.API.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System;

namespace BPLog.API.Tests.Services
{
    /// <summary>
    /// Tests for UserManager service
    /// This test class is uses InMemoryDbFixture. Check what data is in there!
    /// </summary>
    public class UserManagerTests : IClassFixture<InMemoryDbFixture>
    {
        private readonly InMemoryDbFixture _dbFixture;

        private readonly Mock<ILogger<UserManager>> _loggerMock;
        private readonly IConfiguration _configuration;
        private readonly string _privateKey = "my proper private key";

        public UserManagerTests(InMemoryDbFixture dbFixture)
        {
            _loggerMock = new Mock<ILogger<UserManager>>();

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { { "PrivateKey", _privateKey } })
                .Build();

            _dbFixture = dbFixture ?? throw new ArgumentNullException(nameof(dbFixture));
        }

        /// <summary>
        /// Runs bunch of tests to verify that existing user can retrieved by different "types" of logins (whitespaces, lower/upper case letters)
        /// </summary>
        /// <param name="inputLogin"></param>
        /// <param name="expectedLogin"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("charmander", "charmander")]
        [InlineData(" PiKachu", "pikachu")]
        [InlineData(" bulbasauR  ", "bulbasaur")]
        public async Task GetUserByLogin_IfUserExists_ShouldFindUser(string inputLogin, string expectedLogin)
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            var manager = new UserManager(dbContext, _configuration, _loggerMock.Object);

            User result = await manager.GetUserByLogin(inputLogin);

            result.Should().NotBeNull();
            result.Login.Should().Be(expectedLogin);
        }

        /// <summary>
        /// Runs bunch of tests to verify that null is returned if GetUserByLogin method is called for unexisting users or with incorrect arguments
        /// </summary>
        /// <param name="inputLogin"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("fakeLogin")]
        public async Task GetUserByLogin_IfNoUser_ShouldReturnNull(string inputLogin)
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            var manager = new UserManager(dbContext, _configuration, _loggerMock.Object);

            User result = await manager.GetUserByLogin(inputLogin);

            result.Should().BeNull();
        }

        /// <summary>
        /// Runs bunch of tests to verify that a new user can be added to database (no matter to whitespaces, lower/upper case letters)
        /// </summary>
        /// <param name="inputLogin"></param>
        /// <param name="expectedLogin"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(" NEWUSER ", "newuser")]
        [InlineData("MyUser", "myuser")]
        public async Task RegisterUser_IfSuccess_ShouldCreateUser(string inputLogin, string expectedLogin)
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            var manager = new UserManager(dbContext, _configuration, _loggerMock.Object);

            User result = await manager.RegisterUser(inputLogin, _dbFixture.SharedUserPassword);

            result.Should().NotBeNull();
            result.PasswordHash.Should().Be(_dbFixture.SharedUserPassword.ToSha256());
            result.Id.Should().BeGreaterThan(0);
            result.Login.Should().Be(expectedLogin);
        }

        /// <summary>
        /// Runs bunch of tests to verify that null is returned if user tries to register user that already exists or empty credentials are provided
        /// </summary>
        /// <param name="inputLogin"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData(null, null)]
        [InlineData("pikachu", "")]
        public async Task RegisterUser_IfFail_ShouldReturnNull(string inputLogin, string inputPassword)
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            var manager = new UserManager(dbContext, _configuration, _loggerMock.Object);

            User result = await manager.RegisterUser(inputLogin, inputPassword);

            result.Should().BeNull();
        }

        /// <summary>
        /// Runs bunch of tests to verify that authentication works for existing users and valid JWT is generated
        /// </summary>
        /// <param name="inputLogin"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("pikachu")]
        [InlineData("slowpoke")]
        public async Task Authenticate_IfSuccess_ShouldReturnValidJWT(string inputLogin)
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            var manager = new UserManager(dbContext, _configuration, _loggerMock.Object);

            User user = await manager.GetUserByLogin(inputLogin);

            string result = manager.Authenticate(user, _dbFixture.SharedUserPassword);

            result.Should().NotBeNullOrWhiteSpace();

            // Check token validity
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(result);
            bool isValid = DateTime.UtcNow < jsonToken.ValidTo;

            isValid.Should().BeTrue();
        }

        /// <summary>
        /// Runs bunch of tests to verify that null is returned when authentication called for users that don't exist or for existing users but with wrong password
        /// </summary>
        /// <param name="inputLogin"></param>
        /// <param name="inputPassword"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("non-existing_user", "superPass")]
        [InlineData("pikachu", "PikachuPwd")]
        public async Task Authenticate_IfFail_ShouldReturnNull(string inputLogin, string inputPassword)
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            var manager = new UserManager(dbContext, _configuration, _loggerMock.Object);

            User user = await manager.GetUserByLogin(inputLogin);

            string result = manager.Authenticate(user, inputPassword);

            result.Should().BeNull();
        }
    }
}

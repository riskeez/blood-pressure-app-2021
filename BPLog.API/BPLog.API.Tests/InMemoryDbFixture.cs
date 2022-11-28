using BPLog.API.Domain;
using BPLog.API.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPLog.API.Tests
{
    public class InMemoryDbFixture : IDisposable
    {
        private DbContextOptions<BPLogDbContext> _dbOptions;

        /// <summary>
        /// Create and populate in-memory database
        /// </summary>
        public InMemoryDbFixture()
        {
            _dbOptions = new DbContextOptionsBuilder<BPLogDbContext>()
                .UseInMemoryDatabase(databaseName: "in-memory")
                .Options;

            using (var dbContext = new BPLogDbContext(_dbOptions))
            {
                SeedDatabase(dbContext);
                dbContext.SaveChanges();
            }
        }

        public DbContextOptions<BPLogDbContext> ContextOptions => _dbOptions;

        public string SharedUserPassword { get; } = "123";

        public void Dispose() => _dbOptions = null;

        private void SeedDatabase(BPLogDbContext dbContext)
        {
            string pwdHash = SharedUserPassword.ToSha256();

            var dataList = new List<User>();

            var logins = new string[] { "pikachu", "charmander", "squirtle", "bulbasaur", "psyduck", "slowpoke" };
            foreach (string login in logins)
            {
                var newUser = new User
                {
                    Login = login,
                    PasswordHash = pwdHash
                };

                dbContext.Users.Add(newUser);

                var bloodPressures = Enumerable.Range(1, 10)
                .Select(x => new BloodPressure { DateUTC = DateTime.UtcNow.AddHours(-x), Diastolic = 10 * x, Systolic = 5 * x, User = newUser })
                .ToList();

                dbContext.BloodPressures.AddRange(bloodPressures);
            }
        }
    }
}

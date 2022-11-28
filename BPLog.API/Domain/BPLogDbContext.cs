using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPLog.API.Domain
{
    public class BPLogDbContext : DbContext 
    {
        public DbSet<BloodPressure> BloodPressures { get; set; }
        public DbSet<User> Users { get; set; }

        public BPLogDbContext(DbContextOptions<BPLogDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.DisplayName());
            }

            new UserConfiguration().Configure(modelBuilder.Entity<User>());
            new BloodPressureConfiguration().Configure(modelBuilder.Entity<BloodPressure>());
        }
    }
}

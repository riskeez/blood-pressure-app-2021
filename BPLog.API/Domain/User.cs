using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BPLog.API.Domain
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }

        public List<BloodPressure> BloodPressures { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();

            builder.Property(b => b.Login)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(b => b.Login).IsUnique();

            builder.Property(b => b.PasswordHash)
                .IsRequired();

            builder.HasMany(b => b.BloodPressures).WithOne(b => b.User).HasForeignKey(b => b.UserId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

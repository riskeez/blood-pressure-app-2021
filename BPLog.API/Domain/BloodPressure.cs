using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace BPLog.API.Domain
{
    public class BloodPressure
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime DateUTC { get; set; }
        public int Systolic { get; set; }
        public int Diastolic { get; set; }
    }

    public class BloodPressureConfiguration : IEntityTypeConfiguration<BloodPressure>
    {
        public void Configure(EntityTypeBuilder<BloodPressure> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();

            builder.Property(b => b.DateUTC)
                .IsRequired();

            builder.Property(b => b.Systolic)
                .IsRequired();

            builder.Property(b => b.Systolic)
                .IsRequired();

            builder.HasIndex(b => new { b.UserId, b.DateUTC }, "IX_UserID_DateUTC");
        }
    }
}

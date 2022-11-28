using Xunit;
using FluentAssertions;
using BPLog.API.Services;
using BPLog.API.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using System;
using BPLog.API.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BPLog.API.Mappers;

namespace BPLog.API.Tests.Mappers
{
    /// <summary>
    /// Tests for BloodPressureMappers methods
    /// </summary>
    public class BloodPressureMappersTests
    {
        /// <summary>
        /// Runs a test to verify that domain BloodPressure entity is properly converted to DTO object
        /// </summary>
        [Fact]
        public void ToModel_ShouldReturnProperObject()
        {
            BloodPressure dbEntity = new BloodPressure
            {
                Id = 10,
                DateUTC = DateTime.UtcNow,
                Diastolic = 7,
                Systolic = 8,
                UserId = 5
            };

            var expectedResult = new BloodPressureData
            {
                Id = dbEntity.Id,
                DateUTC = dbEntity.DateUTC,
                Diastolic = dbEntity.Diastolic,
                Systolic = dbEntity.Systolic
            };

            BloodPressureData result = dbEntity.ToModel();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }

        /// <summary>
        /// Runs a test to verify that BloodPressure DTO model is properly converted to new domain entity (new - because Id is skipped)
        /// </summary>
        [Fact]
        public void ToNewEntity_ShouldReturnProperObject()
        {
            int userId = 7;

            var model = new BloodPressureData
            {
                Id = 12,
                DateUTC = DateTime.UtcNow,
                Diastolic = 8,
                Systolic = 10
            };

            BloodPressure expectedResult = new BloodPressure
            {
                DateUTC = model.DateUTC,
                Diastolic = model.Diastolic,
                Systolic = model.Systolic,
                UserId = userId
            };

            BloodPressure result = model.ToNewEntity(userId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}

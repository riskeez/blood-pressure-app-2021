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

namespace BPLog.API.Tests.Services
{
    /// <summary>
    /// Tests for BloodPressureService class
    /// This test class is uses InMemoryDbFixture. Check what data is in there!
    /// </summary>
    public class BloodPressureServiceTests : IClassFixture<InMemoryDbFixture>
    {
        private readonly InMemoryDbFixture _dbFixture;
        private readonly Mock<ILogger<BloodPressureService>> _loggerMock;

        public BloodPressureServiceTests(InMemoryDbFixture dbFixture)
        {
            _loggerMock = new Mock<ILogger<BloodPressureService>>();

            _dbFixture = dbFixture ?? throw new ArgumentNullException(nameof(dbFixture));
        }

        /// <summary>
        /// Runs a test to verify that latest blood pressure measurement is returned for existing user
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetLastRecordByUserID_IfUserExists_ShouldReturnLastRecord()
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            User user = await dbContext.Users.Include(x => x.BloodPressures).FirstAsync();
            BloodPressure userLastBP = user.BloodPressures.OrderByDescending(x => x.DateUTC).First();
            BloodPressureData expectedResult = userLastBP.ToModel();

            var bpService = new BloodPressureService(dbContext, _loggerMock.Object);
            BloodPressureData result = await bpService.GetLastRecordByUserId(user.Id);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }

        /// <summary>
        /// Runs bunch of tests to verify that null is returned if latest pressure measurement is requested for a user that does NOT exist
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        public async Task GetLastRecordByUserID_IfNoUser_ShouldReturnNull(int userId)
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            var bpService = new BloodPressureService(dbContext, _loggerMock.Object);

            BloodPressureData result = await bpService.GetLastRecordByUserId(userId);

            result.Should().BeNull();
        }

        /// <summary>
        /// Runs bunch of tests to verify that false is returned if pressure measurement with provided ID doesn't exist
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(0)]
        [InlineData(10000)]
        public async Task DeleteRecordById_IfNotExists_ShouldReturnFalse(int recordId)
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            int expectedCount = dbContext.BloodPressures.Count();
            var bpService = new BloodPressureService(dbContext, _loggerMock.Object);

            bool result = await bpService.DeleteRecordById(recordId);
            int resultCount = dbContext.BloodPressures.Count();

            result.Should().BeFalse();
            resultCount.Should().Be(expectedCount);
        }

        /// <summary>
        /// Runs a test to verify that existing blood pressure measurement record can be properly deleted
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteRecordById_IfExists_ShouldReturnTrue()
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            int deleteId = dbContext.BloodPressures.Last().Id;
            int expectedCount = dbContext.BloodPressures.Count() - 1;

            var bpService = new BloodPressureService(dbContext, _loggerMock.Object);
            bool result = await bpService.DeleteRecordById(deleteId);

            var resultEntity = await dbContext.BloodPressures.FirstOrDefaultAsync(x => x.Id == deleteId);
            int resultCount = dbContext.BloodPressures.Count();

            result.Should().BeTrue();
            resultEntity.Should().BeNull();
            resultCount.Should().Be(expectedCount);
        }

        /// <summary>
        /// Runs a test to verify that a new blood pressure measurement can be saved and linked with existing user
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SaveRecord_IfUserExists_ShouldReturnTrue()
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            User user = await dbContext.Users.FirstAsync();
            int expectedCount = dbContext.BloodPressures.Where(x => x.UserId == user.Id).Count() + 1;

            var bpService = new BloodPressureService(dbContext, _loggerMock.Object);
            bool result = await bpService.SaveRecord(user.Id, new BloodPressureData { DateUTC = DateTime.UtcNow, Diastolic = 5, Systolic = 10 } );

            int resultCount = dbContext.BloodPressures.Where(x => x.UserId == user.Id).Count();

            result.Should().BeTrue();
            resultCount.Should().Be(expectedCount);
        }

        /// <summary>
        /// Runs a test to verify that false is returned if a new blood pressure measurement is being added for user that does NOT exist
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SaveRecord_IfUserNotExists_ShouldReturnFalse()
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            int expectedCount = dbContext.BloodPressures.Count();

            var bpService = new BloodPressureService(dbContext, _loggerMock.Object);

            int notExistingUserId = 0;
            bool result = await bpService.SaveRecord(notExistingUserId, new BloodPressureData { DateUTC = DateTime.UtcNow, Diastolic = 5, Systolic = 10 });

            int resultCount = dbContext.BloodPressures.Count();

            result.Should().BeFalse();
            resultCount.Should().Be(expectedCount);
        }

        /// <summary>
        /// Runs a test to verify that correct page is returned when GetList method is used
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetList_IfUserExists_ShouldReturnProperPage()
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            int pageSize = 2;
            int page = 1;

            User user = await dbContext.Users.Include(x => x.BloodPressures).FirstAsync();
            int expectedPagesCount = Convert.ToInt32(Math.Ceiling(1.0 * user.BloodPressures.Count / pageSize));
            var expectedPressures = user.BloodPressures.OrderByDescending(x => x.DateUTC).Skip(pageSize * page).Take(pageSize).Select(x => x.ToModel()).ToList();

            var bpService = new BloodPressureService(dbContext, _loggerMock.Object);
            BloodPressurePage result = await bpService.GetList(user.Id, pageSize, page, SortOrder.Descending);

            int resultCount = dbContext.BloodPressures.Count();

            result.Should().NotBeNull();
            result.TotalPages.Should().Be(expectedPagesCount);
            result.Payload.Should().BeEquivalentTo(expectedPressures);
        }

        /// <summary>
        /// Runs a test to verify that page with correct data sorted by descending is returned for existing user
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetList_IfUserExists_ShouldReturnSortedDesc()
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            int pageSize = 5;
            int page = 0;

            User user = await dbContext.Users.Include(x => x.BloodPressures).FirstAsync();
            var expectedPressures = user.BloodPressures.OrderByDescending(x => x.DateUTC).Skip(pageSize * page).Take(pageSize).Select(x => x.ToModel()).ToList();

            var bpService = new BloodPressureService(dbContext, _loggerMock.Object);
            BloodPressurePage result = await bpService.GetList(user.Id, pageSize, page, SortOrder.Descending);

            result.Should().NotBeNull();
            result.Payload.Should().BeEquivalentTo(expectedPressures);
        }

        /// <summary>
        /// Runs a test to verify that page with correct data sorted by ascending is returned for existing user
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetList_IfUserExists_ShouldReturnSortedAsc()
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            int pageSize = 5;
            int page = 0;

            User user = await dbContext.Users.Include(x => x.BloodPressures).FirstAsync();
            var expectedPressures = user.BloodPressures.OrderBy(x => x.DateUTC).Skip(pageSize * page).Take(pageSize).Select(x => x.ToModel()).ToList();

            var bpService = new BloodPressureService(dbContext, _loggerMock.Object);
            BloodPressurePage result = await bpService.GetList(user.Id, pageSize, page, SortOrder.Ascending);

            result.Should().NotBeNull();
            result.Payload.Should().BeEquivalentTo(expectedPressures);
        }

        /// <summary>
        /// Runs a test to verify that empty page is returned if GetList method called for a user that does NOT exist
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetList_IfNoUser_ShouldReturnEmptyPage()
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            BloodPressurePage expectedResult = BloodPressurePage.GetEmptyPage();

            int notExistingUserId = 0;

            var bpService = new BloodPressureService(dbContext, _loggerMock.Object);
            BloodPressurePage result = await bpService.GetList(notExistingUserId, 10);

            result.Should().BeEquivalentTo(expectedResult);
        }

        /// <summary>
        /// Runs a test to verify that empty page is returned if GetList method called with page size param <= 0
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetList_IfPageSizeZeroOrLess_ShouldReturnEmptyPage(int pageSize)
        {
            var dbContext = new BPLogDbContext(_dbFixture.ContextOptions);

            User user = await dbContext.Users.FirstAsync();
            BloodPressurePage expectedResult = BloodPressurePage.GetEmptyPage();

            var bpService = new BloodPressureService(dbContext, _loggerMock.Object);
            BloodPressurePage result = await bpService.GetList(user.Id, pageSize);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}

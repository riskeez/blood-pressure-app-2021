using BPLog.API.Domain;
using BPLog.API.Extensions;
using BPLog.API.Mappers;
using BPLog.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BPLog.API.Services
{
    public class BloodPressureService : IBloodPressureService
    {
        private readonly ILogger<BloodPressureService> _logger;
        private readonly BPLogDbContext _dbContext;

        public BloodPressureService(BPLogDbContext dbContext, ILogger<BloodPressureService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<BloodPressureData> GetLastRecordByUserId(int userId, CancellationToken cancellationToken = default)
        {
            var dbEntity = await _dbContext.BloodPressures.Where( x => x.UserId == userId).OrderByDescending(x => x.DateUTC).FirstOrDefaultAsync(cancellationToken);
            if (dbEntity != null)
            {
                return dbEntity.ToModel();
            }
            return null;
        }

        public async Task<bool> DeleteRecordById(int id, CancellationToken cancellationToken = default)
        {
            var dbEntity = await _dbContext.BloodPressures.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (dbEntity != null)
            {
                _dbContext.BloodPressures.Remove(dbEntity);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }

        public async Task<BloodPressurePage> GetList(int userId, int pageSize, int page = 0, SortOrder sortOrder = SortOrder.Descending, CancellationToken cancellationToken = default)
        {
            var resultPage = BloodPressurePage.GetEmptyPage();
            if (pageSize <= 0)
            {
                return resultPage;
            }

            IQueryable<BloodPressure> query = _dbContext.BloodPressures.Where(x => x.UserId == userId);

            var count = await query.CountAsync(cancellationToken);
            int totalPages = Convert.ToInt32(Math.Ceiling(1.0 * count / pageSize));

            query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.DateUTC) : query.OrderBy(x => x.DateUTC);
            query = query.Take(page * pageSize, pageSize);

            var listData = await query.ToListAsync(cancellationToken);

            resultPage.TotalPages = totalPages;
            resultPage.Payload = listData.Select(x => x.ToModel()).ToList();

            return resultPage;
        }

        public async Task<bool> SaveRecord(int userId, BloodPressureData measurement, CancellationToken cancellationToken = default)
        {
            try
            {
                User userEnt = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
                if (userEnt != null)
                {
                    var dbEntity = measurement.ToNewEntity(userId);

                    _dbContext.BloodPressures.Add(dbEntity);

                    int res = await _dbContext.SaveChangesAsync(cancellationToken);
                    return res > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User: {userId}. Record: {record}", userId, JsonConvert.SerializeObject(measurement));
            }
            return false;
        }
    }
}

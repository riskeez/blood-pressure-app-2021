using BPLog.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BPLog.API.Services
{
    /// <summary>
    /// Sorting order
    /// </summary>
    public enum SortOrder
    {
        Ascending = 0,
        Descending = 1
    }

    public interface IBloodPressureService
    {
        /// <summary>
        /// Gets latest recorded blood pressure measurement for a user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Null if no data found</returns>
        Task<BloodPressureData> GetLastRecordByUserId(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a specified blood pressure record
        /// </summary>
        /// <param name="id">Blood Pressure Record Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteRecordById(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets data with pagination
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="sortByDate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Page with data</returns>
        Task<BloodPressurePage> GetList(int userId, int pageSize, int page = 0, SortOrder sortByDate = SortOrder.Descending, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add a new blood pressure record
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="measurement"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> SaveRecord(int userId, BloodPressureData measurement, CancellationToken cancellationToken = default);
    }
}

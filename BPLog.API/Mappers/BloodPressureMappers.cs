using BPLog.API.Domain;
using BPLog.API.Model;

namespace BPLog.API.Mappers
{
    /// <summary>
    /// Mappers that help to convert domain models to DTO models and vice versa
    /// </summary>
    public static class BloodPressureMappers
    {
        /// <summary>
        /// Create DTO from domain entity
        /// </summary>
        /// <param name="entity">Domain level entity</param>
        /// <returns></returns>
        public static BloodPressureData ToModel(this BloodPressure entity)
        {
            return new BloodPressureData
            {
                Id = entity.Id,
                DateUTC = entity.DateUTC,
                Diastolic = entity.Diastolic,
                Systolic = entity.Systolic
            };
        }

        /// <summary>
        /// Create new domain entity from provided model
        /// </summary>
        /// <param name="model">BloodPressure Model</param>
        /// <param name="userId">User</param>
        /// <returns></returns>
        public static BloodPressure ToNewEntity(this BloodPressureData model, int userId)
        {
            return new BloodPressure
            {
                DateUTC = model.DateUTC,
                Diastolic = model.Diastolic,
                Systolic = model.Systolic,
                UserId = userId,
            };
        }
    }
}

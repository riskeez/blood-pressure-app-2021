using System;
using System.ComponentModel.DataAnnotations;

namespace BPLog.API.Model
{
    /// <summary>
    /// Blood Pressure measurement representation
    /// </summary>
    public class BloodPressureData
    {
        /// <summary>
        /// Record Id in database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Record date (UTC)
        /// </summary>
        [Required]
        public DateTime DateUTC { get; set; }

        /// <summary>
        /// Blood overpressure 
        /// </summary>
        [Required]
        public int Systolic { get; set; }

        /// <summary>
        /// Blood underpressure
        /// </summary>
        [Required]
        public int Diastolic { get; set; }
    }
}

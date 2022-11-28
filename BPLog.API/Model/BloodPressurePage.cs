using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPLog.API.Model
{
    /// <summary>
    /// Blood Pressure measurements with pagination
    /// </summary>
    public class BloodPressurePage : PageResponse<BloodPressureData>
    {
        /// <summary>
        /// Gets empty page (Payload is empty array)
        /// </summary>
        /// <returns></returns>
        public static BloodPressurePage GetEmptyPage() => new BloodPressurePage { Payload = Array.Empty<BloodPressureData>(), TotalPages = 0 };
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BPLog.App.Models
{
    public class BloodPressure
    {
        public int Id { get; set; }
        public DateTime DateUTC { get; set; }
        public int Systolic { get; set; }
        public int Diastolic { get; set; }

        public bool IsHighPressure => Systolic > 140 || Diastolic > 90;
    }
}

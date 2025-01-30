using MMSCore.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSCore
{
    public class Booking:Entity
    {
        [Required]
        public DateTime BookingDate { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        [Required]
        public RepeatOption RepetitionOption { get; set; } // "Day", "Week", "NoRepeat"
        public DateTime? EndRepeatedDate { get; set; }
        public DaysofworkEnum? DaysToRepeatedOn { get; set; } // Multiple values store. So enum used by [Flags]
        public DateTime? RequestedOn { get; set; }
        public string? Subject {  get; set; }
        public string? Host {  get; set; }
    }
}

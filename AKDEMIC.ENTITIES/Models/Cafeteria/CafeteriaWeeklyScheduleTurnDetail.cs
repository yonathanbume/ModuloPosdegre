using AKDEMIC.CORE.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class CafeteriaWeeklyScheduleTurnDetail
    {
        public Guid Id { get; set; }
        public byte Type { get; set; } // desayuno, almuerzo, cena
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public Guid CafeteriaWeeklyScheduleId { get; set; }
        public Guid? MenuPlateId { get; set; }

        public MenuPlate MenuPlate { get; set; }
        public CafeteriaWeeklySchedule CafeteriaWeeklySchedule { get; set; }

        [NotMapped]
        public string FormatedStartTime => StartTime.ToLocalDateTimeFormatUtc();
        [NotMapped]
        public string FormatedEndTime => EndTime.ToLocalDateTimeFormatUtc();

        [NotMapped]
        public bool IsActive => StartTime <= DateTime.UtcNow.TimeOfDay && EndTime > DateTime.UtcNow.TimeOfDay;

        public virtual ICollection<UserCafeteriaDailyAssistance> UserCafeteriaDailyAssistances { get; set; }

        //[NotMapped]
        //public bool IsActive => (ConstantHelpers.WEEKDAY.TO_ENUM(DayOfWeek) == DateTime.Now.DayOfWeek && StartTime <= DateTime.Now.TimeOfDay && EndTime >= DateTime.Now.TimeOfDay);
    }
}

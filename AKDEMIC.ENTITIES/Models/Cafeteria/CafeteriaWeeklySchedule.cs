using AKDEMIC.CORE.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class CafeteriaWeeklySchedule
    {
        public Guid Id { get; set; }
        public int DayOfWeek { get; set; }
        public Guid CafeteriaServiceTermScheduleId { get; set; }        
        public CafeteriaServiceTermSchedule CafeteriaServiceTermSchedule { get; set; }



        //public Guid? MenuPlateId { get; set; }
        //public MenuPlate MenuPlate { get; set; }

        //public virtual ICollection<UserCafeteriaDailyAssistance> UserCafeteriaDailyAssistances { get; set; }

        //[NotMapped]
        //public bool IsActive => (ConstantHelpers.WEEKDAY.TO_ENUM(DayOfWeek) == DateTime.Now.DayOfWeek && StartTime <= DateTime.Now.TimeOfDay && EndTime >= DateTime.Now.TimeOfDay);
        [NotMapped]
        public string FormatedDayOfWeek => ConstantHelpers.WEEKDAY.VALUES[DayOfWeek];
       
    }
    //LUNES
    //MARTES
}

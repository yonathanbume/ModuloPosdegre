using System;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class UserCafeteriaDailyAssistance
    {
        public Guid Id { get; set; }
        public Guid UserCafeteriaServiceTermId { get; set; }        
        public Guid CafeteriaWeeklyScheduleTurnDetailId { get; set; }
        public bool IsAbsent { get; set; }
        public UserCafeteriaServiceTerm UserCafeteriaServiceTerm { get; set; }
        public CafeteriaWeeklyScheduleTurnDetail CafeteriaWeeklyScheduleTurnDetail { get; set; }
    }
}

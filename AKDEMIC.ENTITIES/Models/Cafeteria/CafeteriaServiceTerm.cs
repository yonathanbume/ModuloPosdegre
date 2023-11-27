using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class CafeteriaServiceTerm : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }

        public int DaysPerWeek { get; set; }
        public int WeeksPerTerm { get; set; }

        [NotMapped]
        public bool IsActive => DateBegin <= DateTime.UtcNow && DateEnd > DateTime.UtcNow;

        [NotMapped]
        public bool IsFinished => DateEnd <= DateTime.UtcNow;

        public string OwnerId { get; set; }
        public IEnumerable<UserCafeteriaServiceTerm> UserCafeteriaServiceTerms { get; set; }
        public IEnumerable<CafeteriaServiceTermSchedule> CafeteriaServiceTermSchedules { get; set; }

        public IEnumerable<CafeteriaPostulation> CafeteriaPostulations { get; set; }
        //periodo - lista de alumnos 
    }
}

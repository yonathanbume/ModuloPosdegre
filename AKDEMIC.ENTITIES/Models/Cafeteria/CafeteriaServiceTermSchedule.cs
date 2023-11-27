using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    /// <summary>
    /// SEMANAS DEL PERIODO
    /// </summary>
    public class CafeteriaServiceTermSchedule : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid CafeteriaServiceTermId { get; set; }
        public CafeteriaServiceTerm CafeteriaServiceTerm { get; set; }
        public int WeekNumber { get; set; }
        public string Description { get; set; }

        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }

        [NotMapped]
        public bool IsActive => DateBegin <= DateTime.UtcNow && DateEnd > DateTime.UtcNow;

        [NotMapped]
        public bool IsFinished => DateEnd <= DateTime.UtcNow;

        [NotMapped]
        public string Status => IsActive ? "Activa" : (IsFinished ? "Finalizada" : "Pendiente");

        [NotMapped]
        public string FormatedDateBegin => DateBegin.ToLocalDateFormat();

        [NotMapped]
        public string FormatedDateEnd => DateEnd.ToLocalDateFormat();

        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public virtual IEnumerable<CafeteriaWeeklySchedule> CafeteriaWeeklySchedules { get; set; }
        //Semana alimentaria de vegatales
    }
}

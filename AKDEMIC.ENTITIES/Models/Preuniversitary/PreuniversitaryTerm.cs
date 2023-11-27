using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Preuniversitary
{
    public class PreuniversitaryTerm
    {
        public Guid Id { get; set; }

        public int Year { get; set; }

        public int Order { get; set; }

        [NotMapped]
        public string Name => $"{Year}-{Order}";

        [NotMapped]
        public bool IsActive => StartDate <= DateTime.UtcNow && EndDate > DateTime.UtcNow;

        //[NotMapped]
        //public bool IsActivePostulation => PostulantStartDate <= DateTime.UtcNow && PostulantEndDate > DateTime.UtcNow;

        [NotMapped]
        public bool IsFinished => EndDate <= DateTime.UtcNow;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime ClassStartDate { get; set; }

        public DateTime ClassEndDate { get; set; }

        public DateTime PostulantStartDate { get; set; }

        public DateTime PostulantEndDate { get; set; }
    }
}

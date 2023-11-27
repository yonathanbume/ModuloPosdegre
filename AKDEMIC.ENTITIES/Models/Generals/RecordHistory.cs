using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class RecordHistory
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public Guid? StudentId { get; set; }
        public byte Status { get; set; } = ConstantHelpers.RECORD_HISTORY_STATUS.GENERATED;
        public Student Student { get; set; }
        public Guid? RecordTermId { get; set; } //para trámites que requieren un periodo en específico
        public Term RecordTerm { get; set; }
        public string ReceiptCode { get; set; }
        public string DerivedUserId { get; set; }
        public ApplicationUser DerivedUser { get; set; }
        public ICollection<RecordHistoryObservation> RecordHistoryObservations { get; set; }
        public string FileURL { get; set; }
        public int QuantityPrinted { get; set; }

        //Certificado de estudios parcial
        public string JsonAcademicYears { get; set; }

        public int? StartAcademicYear { get; set; }
        public int? EndAcademicYear { get; set; }
        public Guid? StartTermId { get; set; }
        public Guid? EndTermId { get; set; }

        public Term StartTerm { get; set; }
        public Term EndTerm { get; set; }
        public string HtmlContent { get; set; }

        [NotMapped]
        public string Code => $"{Number.ToString().PadLeft(5, '0')}-{Date.Year}";

        public List<UserProcedure> UserProcedures { get; set; }
    }
}

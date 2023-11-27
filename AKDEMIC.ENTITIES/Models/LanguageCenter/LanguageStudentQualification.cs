using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.LanguageCenter
{
    public class LanguageStudentQualification : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public decimal Qualification { get; set; } = 0.00M;
        [Required]
        public string StudentId { get; set; }
        [ForeignKey("StudentId")]
        public ApplicationUser Student { get; set; }
        [Required]
        public Guid LanguageQualificationId { get; set; }
        [ForeignKey("LanguageQualificationId")]
        public LanguageQualification LanguageQualification { get; set; }
    }
}

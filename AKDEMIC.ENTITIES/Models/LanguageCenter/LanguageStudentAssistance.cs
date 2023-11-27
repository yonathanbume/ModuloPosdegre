using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.LanguageCenter
{
    public class LanguageStudentAssistance
    {
        public Guid Id { get; set; }
        public bool IsLate { get; set; } = false;
        public string StudentId { get; set; }
        [ForeignKey("StudentId")]
        public ApplicationUser Student { get; set; }
        public Guid LanguageSectionId { get; set; }
        [ForeignKey("LanguageSectionId")]
        public LanguageSection LanguageSection { get; set; }
        public DateTime DateTime { get; set; }
    }
}

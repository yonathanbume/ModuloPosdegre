using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.LanguageCenter
{
    public class LanguageAcademicHistory
    {
        public Guid Id { get; set; }
        public string StudentId { get; set; }
        [ForeignKey("StudentId")]
        public ApplicationUser Student { get; set; }
        public Guid LanguageSectionId { get; set; }
        [ForeignKey("LanguageSectionId")]
        public LanguageSection LanguageSection { get; set; }
        [Required]
        public byte State { get; set; }
        public string Observations { get; set; } = "";

    }
}

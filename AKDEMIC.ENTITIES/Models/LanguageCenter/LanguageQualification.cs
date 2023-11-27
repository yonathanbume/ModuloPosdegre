using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.LanguageCenter
{
    public class LanguageQualification
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public decimal Qualification { get; set; }
        public Guid LanguageSectionId { get; set; }
        public LanguageSection LanguageSection { get; set; }
    }
}

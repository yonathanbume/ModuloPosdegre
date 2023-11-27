using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.LanguageCenter
{
    public class LanguageSectionStudent
    {
        public Guid Id { get; set; }
        public Guid? UserProcedureId { get; set; }
        [ForeignKey("UserProcedureId")]
        public UserProcedure UserProcedure { get; set; }
        public Guid LanguageSectionId { get; set; }
        [ForeignKey("LanguageSectionId")]
        public LanguageSection LanguageSection { get; set; }
    }
}

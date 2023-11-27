using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.LanguageCenter
{
    public class LanguageLevel : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        public Guid LanguageCourseId { get; set; }
        public LanguageCourse LanguageCourse { get; set; }
        public Guid? PreLanguageLevelId { get; set; }
        public LanguageLevel PreLanguageLevel { get; set; }
    }
}

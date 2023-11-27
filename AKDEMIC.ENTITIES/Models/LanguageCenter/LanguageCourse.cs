using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.LanguageCenter
{
    public class LanguageCourse : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}

using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AKDEMIC.ENTITIES.Models.LanguageCenter
{
    public class LanguageSection : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(5)]
        public string Code { get; set; }
        public Guid LanguageLevelId { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
        public string TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public ApplicationUser Teacher { get; set; }
        public int Vacancies { get; set; } = 0;
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [NotMapped]
        public string StartStringdDate => StartDate.ToDateFormat();
        [NotMapped]
        public string EndStringdDate => EndDate.ToDateFormat();
        public byte State { get; set; }

        public ICollection<LanguageSectionSchedule> LanguageSectionSchedules { get; set; }
        public ICollection<LanguageSectionStudent> LanguageSectionStudents { get; set; }
    }
}

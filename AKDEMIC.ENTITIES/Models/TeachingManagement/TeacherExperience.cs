using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class TeacherExperience : Entity, ISoftDelete, ITimestamp
    {
        [Key]
        public Guid Id { get; set; }
        public int LaborType { get; set; }
        public int LevelPedagogy { get; set; }
        public string InstituteDescription { get; set; }
        public bool State { get; set; }
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
        public string TimeFrame { get; set; }
        public int Relation { get; set; }
        public string FilePath { get; set; }

        public Guid TeacherInformationId { get; set; }
        public virtual TeacherInformation TeacherInformation { get; set; }
    }
}

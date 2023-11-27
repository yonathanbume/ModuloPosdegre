using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.InterestGroup
{
    public class InterestGroup : Entity,ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserAdminId { get; set; }
        public ApplicationUser UserAdmin { get; set; }

        public Guid AcademicProgramId { get; set; }
        public AcademicProgram AcademicProgram { get; set; }

        public ICollection<Meeting> Meetings { get; set; }
        public ICollection<InterestGroupForum> InterestGroupForums { get; set; }
        public ICollection<InterestGroupSurvey> InterestGroupSurveys { get; set; }
        public ICollection<InterestGroupUser> InterestGroupUsers { get; set; }
        public ICollection<InterestGroupFile> InterestGroupFiles { get; set; }

        [NotMapped]
        public string AcademicProgramName { get; set; }
        [NotMapped]
        public string StartFormattedDate { get; set; }

        [NotMapped]
        public bool Active => (StartDate.Date <= DateTime.UtcNow.Date && EndDate.Date >= DateTime.UtcNow.Date) ? true : false;

    }
}
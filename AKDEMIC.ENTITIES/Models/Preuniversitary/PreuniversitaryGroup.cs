using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Preuniversitary
{
    public class PreuniversitaryGroup
    {
        public Guid Id { get; set; }

        public Guid PreuniversitaryCourseId { get; set; }

        public PreuniversitaryCourse PreuniversitaryCourse { get; set; }

        public Guid PreuniversitaryTermId { get; set; }

        public PreuniversitaryTerm PreuniversitaryTerm { get; set; }

        [Required]
        public string Code { get; set; }

        public int Capacity { get; set; } = 0;

        [Required]
        public string TeacherId { get; set; }

        public ApplicationUser Teacher { get; set; }

        public ICollection<PreuniversitarySchedule> PreuniversitarySchedules { get; set; }
        public ICollection<PreuniversitaryUserGroup> PreuniversitaryUserGroups { get; set; }
    }
}

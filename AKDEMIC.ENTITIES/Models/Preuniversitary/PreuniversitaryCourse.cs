using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Preuniversitary
{
    public class PreuniversitaryCourse
    {
        public Guid Id { get; set; }
        public Guid CareerId { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }
        public byte Status { get; set; }

        [NotMapped]
        public string FullName => $"{Code} - {Name}";

        public Career Career { get; set; }

        public ICollection<PreuniversitaryGroup> PreuniversitaryGroups { get; set; }
        public ICollection<PreuniversitaryTemary> PreuniversitaryTemaries { get; set; }
    }
}

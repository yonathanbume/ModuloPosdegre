using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Preuniversitary
{
    public class PreuniversitaryTemary
    {
        public Guid Id { get; set; }

        public Guid PreuniversitaryCourseId { get; set; }

        public PreuniversitaryCourse PreuniversitaryCourse { get; set; }

        public Guid PreuniversitaryTermId { get; set; }

        public PreuniversitaryTerm PreuniversitaryTerm { get; set; }

        [Required]
        public string Topic { get; set; }

        public ICollection<PreuniversitaryAssistance> PreuniversitaryAssistances { get; set; }
    }
}

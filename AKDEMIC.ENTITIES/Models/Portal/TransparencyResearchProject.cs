using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyResearchProject : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public int Year { get; set; }

        [Required]
        public string Slug { get; set; }
        public string ExternalUrl { get; set; }

        public ICollection<TransparencyResearchProjectFile> Files { get; set; }
    }
}

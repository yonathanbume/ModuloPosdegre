using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class DigitalResource : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Tipo { get; set; }
        public string Sorter { get; set; }

        public string FileUrl { get; set; }
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        public ICollection<DigitalResourceCareer> DigitalResourceCareers { get; set; }

        [NotMapped]
        public string CreatedAtFormatted => CreatedAt.ToLocalDateTimeFormat();

        [NotMapped]
        public string Careers { get; set; }
    }
}
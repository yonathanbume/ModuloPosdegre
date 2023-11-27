using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class DigitalResourceCareer
    {
        [Key]
        public Guid CareerId { get; set; }
        [Key]
        public Guid DigitalResourceId { get; set; }
        public DigitalResource DigitalResource { get; set; }
        public Career Career { get; set; }
    }
}

using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class SubImage : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid VQuestionId { get; set; }

        [Required]
        public string ImageRoute { get; set; }

        [RegularExpression(@"[0-9]?[0-9]?[0-9]?(\.[0-9][0-9]?)?")]
        public decimal A { get; set; }

        [RegularExpression(@"[0-9]?[0-9]?[0-9]?(\.[0-9][0-9]?)?")]
        public decimal H { get; set; }

        [RegularExpression(@"[0-9]?[0-9]?[0-9]?(\.[0-9][0-9]?)?")]
        public decimal W { get; set; }

        [RegularExpression(@"[0-9]?[0-9]?[0-9]?(\.[0-9][0-9]?)?")]
        public decimal X { get; set; }

        [RegularExpression(@"[0-9]?[0-9]?[0-9]?(\.[0-9][0-9]?)?")]
        public decimal Y { get; set; }

        public VQuestion VQuestion { get; set; }
    }

    
}
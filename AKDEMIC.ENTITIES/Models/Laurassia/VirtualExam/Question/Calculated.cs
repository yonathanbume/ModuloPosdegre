using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Calculated : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid VQuestionId { get; set; }

        [RegularExpression(@"[0-9]?[0-9]?[0-9]?(\.[0-9][0-9]?)?")]
        [Column(TypeName = "decimal(10, 4)")]
        public decimal Max { get; set; }

        [RegularExpression(@"[0-9]?[0-9]?[0-9]?(\.[0-9][0-9]?)?")]
        [Column(TypeName = "decimal(10, 4)")]
        public decimal Min { get; set; }

        public virtual VQuestion VQuestion { get; set; }
    }
}
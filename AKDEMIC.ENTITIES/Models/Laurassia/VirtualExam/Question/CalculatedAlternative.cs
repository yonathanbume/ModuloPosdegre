using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class CalculatedAlternative : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid VQuestionId { get; set; }
        [Column(TypeName = "decimal(10, 4)")]
        public decimal Calification { get; set; }
        [Column(TypeName = "decimal(10, 4)")]
        public decimal Error { get; set; }
        public string Formula { get; set; } //Is processed in the view(Exam must have a location to get this information)

        public VQuestion VQuestion { get; set; }
    }
}

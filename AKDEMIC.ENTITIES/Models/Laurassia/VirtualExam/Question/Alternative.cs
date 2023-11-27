using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Alternative : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid VQuestionId { get; set; }

        [Required]
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsValid { get; set; }
        public VQuestion VQuestion { get; set; }
        [Column(TypeName = "decimal(8, 5)")]
        public decimal Percentage { get; set; }
    }
}
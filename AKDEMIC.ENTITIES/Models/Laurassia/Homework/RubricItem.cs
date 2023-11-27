using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    [Table("RubricItem")]
    public class RubricItem : Entity, ITimestamp
    {
        [Key]
        public Guid Id { get; set; }
        public Guid HomeworkId { get; set; }
        public virtual Homework Homework { get; set; }
        [Required]
        public string Description { get; set; }
        public ICollection<RubricItemDetail> RubricItemDetails { get; set; }
        [Required]
        public decimal Score { get; set; }
    }
}

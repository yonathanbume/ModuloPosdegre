using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Choice : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid GroupChoiceId { get; set; }

        [Required]
        public string Description { get; set; }
        public bool Selected { get; set; }

        public virtual GroupChoice GroupChoice { get; set; }
    }
}

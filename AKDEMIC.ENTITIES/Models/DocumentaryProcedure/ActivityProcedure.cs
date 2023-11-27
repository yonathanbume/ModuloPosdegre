using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class ActivityProcedure : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        public string Description { get; set; }

    }
}

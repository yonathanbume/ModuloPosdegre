using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class UserSuggestion : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(300)]
        public string Title { get; set; }
        [StringLength(900)]
        public string Description { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}

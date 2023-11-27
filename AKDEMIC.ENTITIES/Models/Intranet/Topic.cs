using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class Topic : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid ForumId { get; set; }

        public string UserId { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        public ApplicationUser User { get; set; }

        public Forum Forum { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
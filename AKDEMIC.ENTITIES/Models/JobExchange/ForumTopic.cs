using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class ForumTopic
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte Category { get; set; }
        public bool IsFavorite { get; set; } = false;
        public string UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<ForumPost> LstForumPosts { get; set; }
    }
}

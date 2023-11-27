using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class ForumPost : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid? PostCitedId { get; set; }
        public Guid ForumTopicId { get; set; }
        public string Message { get; set; }
        public ApplicationUser User { get; set; }
        public ForumPost PostCited { get; set; }
        public ForumTopic ForumTopic { get; set; }
    }
}

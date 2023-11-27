using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class SupportMessage
    {
        public Guid Id { get; set; }
        public Guid SupportChatId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }        
        public DateTime Fecha { get; set; }        
        public ApplicationUser User { get; set; }
        public SupportChat SupportChat { get; set; }
    }
}

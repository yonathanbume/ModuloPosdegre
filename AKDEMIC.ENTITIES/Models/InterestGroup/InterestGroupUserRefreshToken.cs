using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.InterestGroup
{
    public class InterestGroupUserRefreshToken
    {
        public Guid Id { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}

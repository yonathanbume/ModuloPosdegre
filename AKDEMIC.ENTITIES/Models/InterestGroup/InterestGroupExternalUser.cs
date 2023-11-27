using AKDEMIC.ENTITIES.Models.Generals;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.InterestGroup
{
    public class InterestGroupExternalUser
    {
        [Key]
        public string UserId { get; set; }
        public string Origin { get; set; }
        public string Position { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models
{
    public class Connection
    {
        [Key]
        public Guid ConnectionId { get; set; }
        public string UserId { get; set; }

        public string Code { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }   

    }
}

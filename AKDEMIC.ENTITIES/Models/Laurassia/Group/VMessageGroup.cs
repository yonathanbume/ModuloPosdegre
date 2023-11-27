using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    [Table("VMessageGroup")]
    public class VMessageGroup : Entity, ITimestamp
    {
        [Key]
        public Guid Id { get; set; }

        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public String Name { get; set; }

        public String UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public Guid VGroupId { get; set; }

        [ForeignKey("VGroupId")]
        public virtual VGroup VGroup { get; set; }
    }
}

using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Login : Entity, ITimestamp
    {
        public string Id { get; set; }
        public Guid? SectionId { get; set; }
        public string UserId { get; set; }
        public DateTime DateLogin { get; set; }
        public DateTime? DateLogout { get; set; }
        public String Ip { get; set; }
        public String Ua { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser AUser { get; set; }
        public ICollection<LoginDetail> LoginDetails { get; set; }
    }
}

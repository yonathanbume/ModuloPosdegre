using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Server
{
    public class GeneralLinkRole
    {
        [Key]
        public string ApplicationRoleId { get; set; }

        [Key]
        public Guid GeneralLinkId { get; set; }

        public GeneralLink GeneralLink { get; set; }
        public ApplicationRole ApplicationRole { get; set; }
    }
}

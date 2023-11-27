using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.InterestGroup
{
    public class InterestGroupForum
    {
        public Guid Id { get; set; }
        public Guid InterestGroupId { get; set; }
        public InterestGroup InterestGroup { get; set; }
        public Guid ForumId { get; set; }
        public Forum Forum { get; set; }

        public string UserOwnerId { get; set; }
        public ApplicationUser UserOwner { get; set; }

        [NotMapped]
        public string ForumName { get; set; }
        [NotMapped]
        public string Description { get; set; }
        [NotMapped]
        public string AcademicProgramName { get; set; }
        [NotMapped]
        public bool State { get; set; }
    }
}

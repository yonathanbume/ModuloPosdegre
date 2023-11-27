using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.ENTITIES.Models.Permission;
using AKDEMIC.ENTITIES.Models.Tutoring;
using Microsoft.AspNetCore.Identity;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class ApplicationRole : IdentityRole
    {
        public int Priority { get; set; }

        [NotMapped]
        public bool IsInProcedure { get; set; }

        public bool IsStatic { get; set; } = false;

        public ICollection<ProcedureRole> ProcedureRoles { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        public ICollection<RolAnnouncement> RolAnnouncement { get; set; }

        public ICollection<RolePermission> RolePermission { get; set; }

        public ICollection<TutoringAnnouncementRole> TutoringAnnouncementRoles { get; set; } 
    }
}

using System;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Permission
{
    public class RolePermission
    {
        public Guid Id { get; set; }

        public string RoleId { get; set; }

        public string PermissionLabel { get; set; }

        public int Permission { get; set; }

        public ApplicationRole Role { get; set; }
    }
}

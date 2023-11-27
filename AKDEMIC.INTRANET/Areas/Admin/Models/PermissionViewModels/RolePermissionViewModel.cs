using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using AKDEMIC.CORE.Helpers;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.PermissionViewModels
{
    public class RolePermissionViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Nombre", Prompt = "Nombre del rol")]
        public string Name { get; set; }

        public List<Permission> ListPermissions { get; set; }

        public List<SelectListItem> Permissions { get; set; }

        public RolePermissionViewModel()
        {
            Permissions = Enum.GetValues(typeof(ConstantHelpers.PermissionHelpers.Permission))
                                                                .Cast<Enum>()
                                                                .Select(e => new SelectListItem()
                                                                {
                                                                    Value = e.ToString(),
                                                                    Text = EnumHelpers.GetDescription(e)
                                                                })
                                                                .ToList();
        }
    }

    public class Permission
    {
        public string Value { get; set; }
    }
}

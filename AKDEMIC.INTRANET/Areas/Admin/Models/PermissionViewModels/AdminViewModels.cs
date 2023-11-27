using AKDEMIC.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.PermissionViewModels
{
    public class AdminViewModels
    {
        public Guid? Id { get; set; }

        [Required]
        public string TeacherId { get; set; }

        public IDictionary<int, string> ListSystems = GeneralHelpers.GetSystems();

        [Required]
        public List<int> ListChecked { get; set; }
    }
}

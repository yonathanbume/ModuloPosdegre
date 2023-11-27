// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.StudentPortfolioTypeModels
{
    public class TypeViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Dependencia")]
        public Guid? DependencyId { get; set; }
        [Display(Name = "Tipo")]
        public byte Type { get; set; }
        [Display(Name = "¿Puede subirlo el estudiante?")]
        public bool CanUploadStudent { get; set; }
    }
}

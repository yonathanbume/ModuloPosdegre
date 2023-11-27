// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ExtracurricularAreaViewModels
{
    public class ExtracurricularAreaViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Nombre del área")]
        public string Name { get; set; }

        [Display(Name = "Tipo")]
        public byte Type { get; set; }
    }
}

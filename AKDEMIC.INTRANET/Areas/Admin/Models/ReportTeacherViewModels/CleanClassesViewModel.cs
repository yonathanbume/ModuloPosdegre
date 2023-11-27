// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ReportTeacherViewModels
{
    public class CleanClassesViewModel
    {
        public List<Guid> ClassesId { get; set; }
        public Guid SectionId { get; set; }
    }
}

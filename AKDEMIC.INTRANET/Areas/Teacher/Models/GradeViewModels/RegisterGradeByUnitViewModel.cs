// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.GradeViewModels
{
    public class RegisterGradeByUnitViewModel
    {
        public Guid SectionId { get; set; }
        public Guid CourseUnitId { get; set; }
        public List<EvaluationViewModel> Evaluations { get; set; }
        public List<StudentSectionViewModel> StudentSections { get; set; }
    }
}

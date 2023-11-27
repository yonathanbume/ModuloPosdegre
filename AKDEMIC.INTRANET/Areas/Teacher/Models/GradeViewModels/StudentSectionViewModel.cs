// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.GradeViewModels
{
    public class StudentSectionViewModel
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public int Status { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public decimal AbsencesPercentage { get; set; }
        public List<EvaluationViewModel> Evaluations { get; set; }
    }
}

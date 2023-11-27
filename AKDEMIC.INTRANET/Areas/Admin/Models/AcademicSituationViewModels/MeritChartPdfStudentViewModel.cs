// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.AcademicSituationViewModels
{
    public class MeritChartPdfStudentViewModel
    {
        public int Number { get; set; }

        public int MeritOrder { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string AcademicProgram { get; set; }

        public decimal AverageGrade { get; set; }

        public string Curriculum { get; set; }

        public decimal CurriculumCredits { get; set; }

        public decimal EnrolledCredits { get; set; }

        public decimal ApprovedCredits { get; set; }

        public decimal DisapprovedCredits { get; set; }

        public string Modality { get; set; }

        public string Condition { get; set; }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Report.ViewModels.MeritChartViewModels
{
    public class MeritChartPdfViewModel
    {
        public string HeaderText { get; set; }

        public string ChartName { get; set; }

        public string ChartAdditionalText { get; set; }

        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Faculty { get; set; }

        public string Career { get; set; }

        public string Campus { get; set; }

        public string AcademicYear { get; set; }

        public string Term { get; set; }

        public byte GradeType { get; set; }

        public int TotalStudents { get; set; }

        public List<MeritChartPdfStudentViewModel> Students { get; set; }
    }
}

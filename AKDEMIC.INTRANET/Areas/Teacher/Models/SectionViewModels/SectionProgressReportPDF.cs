// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.SectionViewModels
{
    public class SectionProgressReportPDF
    {
        public string HeaderText { get; set; }
        public string Image { get; set; }

        public string Career { get; set; }
        public string AcademicDepartment { get; set; }
        public string Teacher { get; set; }
        public string Course { get; set; }
        public string Term { get; set; }
        public string Curriculum { get; set; }
        public int AcademicYear { get; set; }
        public string Section { get; set; }
        public int Enrolled { get; set; }
        public int HT { get; set; }
        public int HP { get; set; }
        public decimal Progress { get; set; }
        public List<SectionProgressDetailReportPDF> Details { get; set; }
    }

    public class SectionProgressDetailReportPDF
    {
        public DateTime? Date { get; set; }
        public string Subject { get; set; }
        public int Students { get; set; }
        public string Observation { get; set; }
        public int SessionType { get; set; }
    }
}

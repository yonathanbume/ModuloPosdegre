// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace AKDEMIC.PDF.Services.ReportCardGenerator.Models
{
    public class StudentModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Career { get; set; }
        public string Campus { get; set; }
        public decimal? Average { get; set; }
        public string Curriculum { get; set; }
        public string AdmissionType { get; set; }
        public int? AcademicYear { get; set; }
        public decimal? PPS { get; set; }
        public decimal? PPA { get; set; }
        public List<CourseModel> Courses { get; set; }
    }
}

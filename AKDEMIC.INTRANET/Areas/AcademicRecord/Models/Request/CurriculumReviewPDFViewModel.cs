// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Office2010.ExcelAc;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Models.Request
{
    public class CurriculumReviewPDFViewModel
    {
        public string Header { get; set; }
        public string SubHeader { get; set; }
        public string LogoImg { get; set; }
        public string Curriculum { get; set; }
        public string Career { get; set; }
        public string Faculty { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Document { get; set; }
        public decimal RequiredCreditsElective { get; set; }
        public decimal RequiredCredits { get; set; }
        public List<CurriculumReviewCoursesViewModel> Courses { get; set; }
    }

    public class CurriculumReviewCoursesViewModel
    {
        public Guid CourseId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int AcademicYear { get; set; }
        public decimal Credits { get; set; }
        public string Term { get; set; }
        public int? Grade { get; set; }
        public bool Approved { get; set; }
        public bool Validated { get; set; }
        public bool IsElective { get; set; }
    }
}

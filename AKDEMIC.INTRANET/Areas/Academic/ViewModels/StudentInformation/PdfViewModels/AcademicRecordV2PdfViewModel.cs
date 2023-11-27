// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation.PdfViewModels
{
    public class AcademicRecordV2PdfViewModel
    {
        public string ImagePathLogo { get; set; }
        public string Career { get; set; }
        public string UserName { get; set; }
        public string Campus { get; set; }
        public string FullName { get; set; }
        public string Curriculum { get; set; }
        public string Faculty { get; set; }
        public decimal TotalApprovedCredits { get; set; }
        public List<AcademicrecordV2AcademicYearPdfViewModel> AcademicYears { get; set; }
        public List<AcademicRecordV2ExtracurricularAreaViewModel> ExtracurricularAreas { get; set; }
    }

    public class AcademicrecordV2AcademicYearPdfViewModel
    {
        public int AcademicYear { get; set; }
        public string AccumulatedAverageFormula { get; set; }
        public decimal AccumulatedAverage { get; set; }
        public decimal SemesterAverage { get; set; }
        public string SemesterAverageFormula { get; set; }
        public List<AcademicRecordV2HistoryPdfViewModel> AcademicHistories { get; set; }
    }

    public class AcademicRecordV2HistoryPdfViewModel
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Term { get; set; }
        public int Grade { get; set; }
        public string Date { get; set; }
        public decimal Credits { get; set; }
        public byte AcademicHistoryType { get; set; }
        public string Observation { get; set; }
        public int Try { get; set; }
    }

    public class AcademicRecordV2ExtracurricularAreaViewModel
    {
        public string Name { get; set; }
        public List<AcademicRecordv2ExtracurricularActivityViewModel> Activities { get; set; }
    }

    public class AcademicRecordv2ExtracurricularActivityViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Term { get; set; }
        public decimal Credits { get; set; }
        public int? Grade { get; set; }
        public string Type { get; set; }
        public int Try { get; set; }
        public string Date { get; set; }
    }
}

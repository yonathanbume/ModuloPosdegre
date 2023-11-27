// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AKDEMIC.PDF.Services.CompleteCurriculumGenerator.Models
{
    public class CourseModel
    {
        public int AcademicYear { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Credits { get; set; }
        public string TermName { get; set; }
        public int? Grade { get; set; }
    }
}

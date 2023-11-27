// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AKDEMIC.PDF.Services.CompleteCurriculumGenerator.Models
{
    public class StudentModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Campus { get; set; }
        public string AdmissionTerm { get; set; }
        public string Curriculum { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal LastWeightedAverageCumulative { get; set; }
    }
}

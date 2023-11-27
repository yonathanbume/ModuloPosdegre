// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace AKDEMIC.PDF.Services.CertificateOfStudiesGenerator.Models
{
    public class StudentModel
    {
        public string Faculty { get; set; }
        public string Career { get; set; }
        public decimal RequiredCredits { get; set; }
        public string FullName { get; set; }
        public string Surnames { get; set; }
        public string Names { get; set; }
        public string AcademicProgram { get; set; }
        public string Document { get; set; }
        public string UserName { get; set; }
        public string Campus { get; set; }
        public string Curriculum { get; set; }
        public string CurriculumYear { get; set; }
        public decimal WeightedAverageGrade { get; set; }
        public decimal WeightedAverageCumulative { get; set; }
        public decimal PPG { get; set; }
        public string Photo { get; set; }
        public List<StudentObservationModel> StudentObservations { get; set; }
    }
}

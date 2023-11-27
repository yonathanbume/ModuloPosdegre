// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AKDEMIC.PDF.Models;
using System.Collections.Generic;

namespace AKDEMIC.PDF.Services.CertificateOfStudiesGenerator.Models
{
    public class CertificateOfStudiesModel
    {
        //public string HeaderText { get; set; }
        //public string SubHeaderText { get; set; }
        public string UserProcedureCode { get; set; }
        public string Img { get; set; }
        public string ImgQR { get; set; }
        public string User { get; set; }
        public List<AcademicYearCourseModel> AcademicYearCourses { get; set; }
        public UniversityInformationModel University { get; set; }
        public StudentModel Student { get; set; }
    }
}

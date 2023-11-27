// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AKDEMIC.CORE.Helpers;
using System;

namespace AKDEMIC.PDF.Services.CertificateOfStudiesGenerator.Models
{
    public class CourseModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }
        public string GradeText => ConstantHelpers.GRADES.TEXT.ContainsKey(Grade) ? ConstantHelpers.GRADES.TEXT[Grade] : "-";
        public decimal Credits { get; set; }
        public string Term { get; set; }
        public string Observation { get; set; }
        public bool IsElective { get; set; }
        public bool Validated { get; set; }
        public byte Type { get; set; }
        public DateTime? DateByConfiguration { get; set; }
    }
}

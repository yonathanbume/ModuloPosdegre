// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AKDEMIC.INTRANET.Areas.Student.Models.AbsencesViewModels
{
    public class StudentClassReportViewModel
    {
        public Guid SectionId { get; set; }
        public Guid? SectionGroupId { get; set; }
        public string CourseName { get; set; }
        public int ClassCount { get; set; }
        public decimal MaxAbsences { get; set; }
        public int Dictated { get; set; }
        public int Assisted { get; set; }
        public int Absences { get; set; }
        public bool IsActive { get; set; }
        public decimal AbsencesPercentage { get; set; }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.ViewModels.ProcedureViewModels
{
    public class ActivityViewModel
    {
        public byte? Type { get; set; }
        public string Career { get; set; }
        public Guid? CareerId { get; set; }
        public string StudentSection { get; set; }
        public Guid? StudentSectionId { get; set; }
        public string JsonStudentSectionIds { get; set; }

        public List<string> StudentSectionsDescription { get; set; }

        public string AcademicProgram { get; set; }
        public Guid? AcademicProgramId { get; set; }
        public string Curriculum { get; set; }
        public Guid? CurriculumId { get; set; }

        //Para Curso Exonerado / Evalaución Extraordinaria
        public Guid? CourseId { get; set; }
        public string Course { get; set; }
    }
}

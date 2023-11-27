// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class ChangeCurriculumViewModel
    {
        public Guid CurriculumId { get; set; }
        public Guid? AcademicProgramId { get; set; }
        public string Reason { get; set; }
    }
}


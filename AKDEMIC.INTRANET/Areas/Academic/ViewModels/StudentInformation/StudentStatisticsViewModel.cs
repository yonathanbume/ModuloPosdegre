// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class StudentStatisticsViewModel
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Picture { get; set; }
        public List<StudentStatisticsCompetenciesViewModel> Competencies { get; set; }
    }
}

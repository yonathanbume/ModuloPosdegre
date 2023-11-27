// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.EnrollmentReservationViewModels
{
    public class EnrollmentReservationPDF
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Dni { get; set; }
        public string AcademicProgram { get; set; }
        public string Modality { get; set; }
        public string Date { get; set; }
        public string CurrentAcademicYear { get; set; }
        public string Observation { get; set; }
        public string Curriculum { get; set; }
    }
}

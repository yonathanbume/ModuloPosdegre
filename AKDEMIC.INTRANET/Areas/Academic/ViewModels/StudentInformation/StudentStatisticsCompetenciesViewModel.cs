// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class StudentStatisticsCompetenciesViewModel
    {
        public Guid CompetencieId { get; set; }
        public string Competencie { get; set; }
        public string Description { get; set; }
        public byte Type { get; set; }
        public string Average =>
            Details != null && Details.Sum(y => y.Credits) == 0 ?
            "-" :
            Math.Round((Details.Where(y => y.FinalGrade.HasValue).Sum(y => y.FinalGrade.Value * y.Credits) / Details.Where(y => y.FinalGrade.HasValue).Sum(y => y.Credits)), 2).ToString("0.##");
        public string Level { get; set; }
        public List<StudentStatisticsCompetenciesDetailViewModel> Details { get; set; }
    }

    public class StudentStatisticsCompetenciesDetailViewModel
    {
        public string Course { get; set; }
        public string Code { get; set; }
        public decimal Credits { get; set; }
        public int? FinalGrade { get; set; }
        public bool? Approved { get; set; }
    }
}

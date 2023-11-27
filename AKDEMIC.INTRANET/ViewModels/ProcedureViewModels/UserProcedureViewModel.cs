// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.ViewModels.ProcedureViewModels
{
    public class UserProcedureViewModel
    {
        public string CreatedAt { get; set; }
        public Guid Id { get; set; }
        public ProcedureViewModel Procedure { get; set; }
        public PaymentViewModel Payment { get; set; }
        public string Term { get; set; }
        public string Comment { get; set; }
        public string Dependency { get; set; }
        public string StatusStr { get; set; }
        public int Status { get; set; }
        public string UrlImage { get; set; }
        public List<RequirementViewModel> UserRequirementFiles { get; set; }

        //RecordHistory
        public Guid? RecordHistoryId { get; set; }
        public int? RecordHistoryType { get; set; }
        public byte? RecordHistoryRangeType { get; set; }
        public string RecordHistoryStartTerm { get; set; }
        public string RecordHistoryEndTerm { get; set; }
        public int? RecordHistoryStartAcademicYear { get; set; }
        public int? RecordHistoryEndAcademicYear { get; set; }
        public string RecordHistoryTerm { get; set; }
        public string RecordHistoryFileUrl { get; set; }
        public ActivityViewModel Activity { get; set; }
    }
}

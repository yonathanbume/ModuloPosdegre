// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.ViewModels.ProcedureViewModels
{
    public class ProcedureViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool StartDependencyEqualToDepedency { get; set; }
        public string LegalBase { get; set; }
        public string Code { get; set; }
        public string StartDependency { get; set; }
        public string Dependency { get; set; }
        public Guid? ConceptId { get; set; }
        public string Concept { get; set; }
        public decimal? ConceptAmount { get; set; }
        public int Duration { get; set; }
        public bool NeedImage { get; set; }
        public string PendingPaymentTitle { get; set; }

        public ConfigurationViewmodel Configuration { get; set; }

        //Student
        public string StudentComment { get; set; }
        public string UserCroppedImage { get; set; }
        public PaymentViewModel Payment { get; set; }
        public RecordHistoryViewModel RecordHistory { get; set; }
        public ActivityViewModel Activity { get; set; }
        public List<RequirementViewModel> Requirements { get; set; }
    }
}

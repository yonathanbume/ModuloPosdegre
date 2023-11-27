// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.DebtManagementViewModels
{
    public class ConceptViewModel
    {
        public Guid ConceptId { get; set; }

        public decimal Price { get; set; }

        public string Comment { get; set; }

        public string UserCode { get; set; }

        public bool IsEnrollment { get; set; }
    }
}

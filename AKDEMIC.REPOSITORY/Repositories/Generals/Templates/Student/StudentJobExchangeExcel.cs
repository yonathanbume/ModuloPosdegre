// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.JOBEXCHANGE.Areas.Admin.ViewModels.ReportViewModel
{
    public class StudentJobExchangeExcel
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Career { get; set; }
        public string Phone { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Company { get; set; }
        public string Sector { get; set; }
        public string JournalTime { get; set; }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using DocumentFormat.OpenXml.Office2010.ExcelAc;

namespace AKDEMIC.INTRANET.ViewModels.ProcedureViewModels
{
    public class PendingPaymentPdfViewModel
    {
        public string Img { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Term { get; set; }
        public string Career { get; set; }
        public string Title { get; set; }
        public string FooterText { get; set; }
        public decimal Total { get; set; }
        public List<PendingPaymentPdfConceptViewModel> Concepts { get; set; }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AKDEMIC.INTRANET.ViewModels.ProcedureViewModels
{
    public class PaymentViewModel
    {
        public Guid Id { get; set; }
        public decimal Discount { get; set; }
        public decimal IgvAmount { get; set; }
        public decimal SubTotal { get; set; }
        public string Concept { get; set; }
        public string StatusStr { get; set; }

        public decimal Total { get; set; }
        public string OperationCode { get; set; }
        public string PaymentDate { get; set; }
    }
}

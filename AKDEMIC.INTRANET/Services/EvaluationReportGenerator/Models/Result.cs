// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AKDEMIC.INTRANET.Services.EvaluationReportGenerator.Models
{
    public class Result
    {
        public string PdfName { get; set; }
        public byte[] Pdf { get; set; }
    }
}

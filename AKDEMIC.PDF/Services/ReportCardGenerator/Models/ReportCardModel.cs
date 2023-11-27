// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AKDEMIC.PDF.Services.ReportCardGenerator.Models
{
    public class ReportCardModel
    {
        public string ImgPathLogo { get; set; }
        public string HeaderText { get; set; }
        public string UserProcedureCode { get; set; }
        public bool IsProcedure { get; set; }
        public string Term { get; set; }
        public string QrCode { get; set; }
        public bool IsActiveTerm { get; set; }
        public string SignatuareImgBase64 { get; set; }
        public StudentModel Student { get; set; }
    }
}

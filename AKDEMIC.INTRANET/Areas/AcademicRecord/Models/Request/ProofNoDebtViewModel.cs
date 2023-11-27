// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection.Metadata.Ecma335;
using AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Models.Request
{
    public class ProofNoDebtViewModel
    {
        public string Image { get; set; }
        public string SubHeader { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public int StudentSex { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public string Career { get; set; }
        public string Faculty { get; set; }
        public string Correlative { get; set; }
        public UniversityInformation University { get; set; }
    }
}

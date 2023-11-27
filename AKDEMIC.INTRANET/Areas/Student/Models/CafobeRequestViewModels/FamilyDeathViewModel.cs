// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.INTRANET.Areas.Student.Models.CafobeRequestViewModels
{
    public class FamilyDeathViewModel
    {
        public Guid Id { get; set; }

        public Guid? StudentId { get; set; }

        public Guid? TermId { get; set; }

        public int? Type { get; set; }

        public int? Status { get; set; }
        public string Observation { get; set; }

        public IFormFile DirectorRequestFile { get; set; }
        public string DirectorRequestUrl { get; set; }

        public IFormFile DocumentaryProcedureVoucherFile { get; set; }
        public string DocumentaryProcedureVoucherUrl { get; set; }

        public IFormFile LastTermHistoriesFile { get; set; }
        public string LastTermHistoriesUrl { get; set; }

        public IFormFile DniFile { get; set; }
        public string DniUrl { get; set; }

        public IFormFile EnrollmentFormFile { get; set; }
        public string EnrollmentFormUrl { get; set; }

        //propio

        public IFormFile DeathCertificateFile { get; set; }
        public string DeathCertificateUrl { get; set; }

        public IFormFile StudentBirthCertificateFile { get; set; }
        public string StudentBirthCertificateUrl { get; set; }

        //Para Descargar
        public string SystemUrl { get; set; }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.INTRANET.Areas.Student.Models.GradeCorrectionViewModels
{
    public class GradeCorrectionViewModel
    {
        public Guid GradeId { get; set; }
        public string Observations { get; set; }
        public IFormFile File{ get; set; }
    }
}

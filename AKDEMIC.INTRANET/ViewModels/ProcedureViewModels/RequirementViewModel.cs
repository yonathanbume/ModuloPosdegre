// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.INTRANET.ViewModels.ProcedureViewModels
{
    public class RequirementViewModel
    {
        public Guid Id { get; set; }

        public int Status { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal? Cost { get; set; }
        public byte Type { get; set; }
        public byte? SystemValidationType { get; set; }

        //Student
        public IFormFile File { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
    }
}

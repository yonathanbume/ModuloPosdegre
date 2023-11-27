// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.CorrectionExamViewModels
{
    public class CorrectionExamViewModel
    {
        public Guid? Id { get; set; }
        public Guid SectionId { get; set; }
        public string Section { get; set; }
        public Guid? ClassroomId { get; set; }
        public string Classroom { get; set; }
        public IFormFile File { get; set; }
        public string FileUrl { get; set; }
        public IFormFile FileResolution { get; set; }
        public string FileResolutionUrl { get; set; }
        public string TeacherId { get; set; }
        public string Teacher { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndDate { get; set; }

        public string Term { get; set; }
        public int Duration { get; set; }
        public string Course { get; set; }
    }
}

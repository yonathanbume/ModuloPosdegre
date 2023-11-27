// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.DeferredExamViewModels
{
    public class DeferredExamViewModel
    {
        public Guid? Id { get; set; }
        public Guid SectionId { get; set; }
        public Guid? ClassroomId { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public double Duration { get; set; }

        public string Section { get; set; }
        public string Course { get; set; }
        public string Classroom { get; set; }
        public string EndDate { get; set; }
    }
}

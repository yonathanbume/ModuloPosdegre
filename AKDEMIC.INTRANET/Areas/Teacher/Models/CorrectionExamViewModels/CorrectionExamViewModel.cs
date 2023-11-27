// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.CorrectionExamViewModels
{
    public class CorrectionExamViewModel
    {
        public Guid Id { get; set; }
        public string Section { get; set; }
        public string Course { get; set; }
        public string Term { get; set; }
        public string Resolution { get; set; }
        public string File { get; set; }
    }
}

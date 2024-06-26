﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.DeferredExamViewModels
{
    public class DeferredExamViewModel
    {
        public Guid Id { get; set; }
        public string Section { get; set; }
        public string Course { get; set; }
        public string Term { get; set; }
    }
}

﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AKDEMIC.INTRANET.Areas.Admin.Models.DocumentFormatViewModels
{
    public class DocumentFormatViewModel
    {
        public byte Id { get; set; }
        public byte Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AKDEMIC.DEGREE.Areas.Admin.Models.ProjectDirectorViewModels
{
    public class ProjectDirectorViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Surnames { get; set; }

        public string IdentificationCard { get; set; }
        public string PhoneNumber { get; set; }
        public byte Sex { get; set; }
        public Guid CountryId { get; set; }
        public Guid DepartmentId { get; set; }
        public byte CivilStatus { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}

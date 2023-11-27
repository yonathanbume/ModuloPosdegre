using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class SupportOffice
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<SupportOfficeUser> SupportOfficeUsers { get; set; }

        public ICollection<TutoringSessionStudent> TutoringSessionStudents { get; set; }
    }
}

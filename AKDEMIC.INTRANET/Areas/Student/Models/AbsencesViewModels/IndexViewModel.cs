using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Student.Models.AbsencesViewModels
{
    public class IndexViewModel
    {
        public StudentViewModel Student { get; set; }

        public Guid ActiveTerm { get; set; }

        public float AttendanceMinPercentage { get; set; }

        public IEnumerable<TermViewModel> Terms { get; set; }
    }

    public class TermViewModel
    {
        public string Name { get; set; }

        public Guid Id { get; set; }
    }

    public class StudentViewModel
    {
        public string FullName { get; set; }

        public string UserName { get; set; }

        public Guid StudentId { get; set; }

        public CareerViewModel Career { get; set; }
    }

    public class CareerViewModel
    {
        public string Name { get; set; }
    }
}

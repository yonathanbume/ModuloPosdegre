using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class GraduatedFiltersTemplate
    {
        public List<GradeSelect2> Grados { get; set; }
        public List<CareerSelect2> Carreras { get; set; }
        public List<string> Años { get; set; }
    }

    public class GradeSelect2
    {
        public int Type { get; set; }
        public string Description { get; set; }
    }

    public class CareerSelect2
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}

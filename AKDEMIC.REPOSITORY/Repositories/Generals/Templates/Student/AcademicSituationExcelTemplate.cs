using System;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class AcademicSituationExcelTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Dni { get; set; }
        public string Career { get; set; }
        public string Faculty { get; set; }
        public string LastOrder { get; set; }
        public string AprrovedCredits { get; set; }
        public string LastGrade { get; set; }
        public int LastMeritOrder { get; set; }
    }
}

using AKDEMIC.ENTITIES.Models.Enrollment;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicHistory
{
    public class AcademicHistoryTemplate
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Career { get; set; }
        public string AcademicProgram { get; set; }
        public bool Approbed { get; set; }
        public int Grade { get; set; }
        public int Intents { get; set; }
        public IEnumerable<Curriculum> Curriculums { get; set; }
    }
    public class AcademicHistoryReportTemplate
    {
        public string approbedname { get; set; }
        public int approbeds { get; set; }
        public string disapprobedname { get; set; }
        public int disapprobeds { get; set; }
    }
}

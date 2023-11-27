using System;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.AcademicProgram
{
    public class AcademicProgramModelATemplate
    {
        public Guid Id { get;  set; }
        public Guid CareerId { get;  set; }
        public string CareerName { get;  set; }
        public string Name { get;  set; }
        public string Code { get;  set; }
        public string SuneduCode { get;  set; }
        public int Type { get;  set; }
        public string IsProgram { get; set; }
    }
}

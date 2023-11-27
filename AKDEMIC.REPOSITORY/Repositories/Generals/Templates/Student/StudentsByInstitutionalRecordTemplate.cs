using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class StudentsByInstitutionalRecordTemplate
    {
        public string User { get;  set; }
        public string Email { get;  set; }
        public string UserName { get;  set; }
        public string CareerName { get; set; }
        public string CategorizationName { get;  set; }
        public int FinalScore { get;  set; }
        public string SisfohClasification { get;  set; }
        public bool SisfohClasificationExists { get; set; }
        public string SisfohConstancy { get;  set; }
        public Guid Id { get; set; }
    }
}

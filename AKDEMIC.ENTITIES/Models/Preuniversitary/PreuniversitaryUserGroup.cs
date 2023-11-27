using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Preuniversitary
{
    public class PreuniversitaryUserGroup
    {
        public Guid Id { get; set; }
        public Guid PreuniversitaryGroupId { get; set; }
        public string ApplicationUserId { get; set; }
        public PreuniversitaryGroup PreuniversitaryGroup { get; set; }
        public AKDEMIC.ENTITIES.Models.Generals.ApplicationUser ApplicationUser { get; set; }
        public int Grade { get; set; } = 0;
        public bool IsQualified { get; set; }


        public ICollection<PreuniversitaryAssistanceStudent> PreuniversitaryAssistanceStudents { get; set; } 
    }
}

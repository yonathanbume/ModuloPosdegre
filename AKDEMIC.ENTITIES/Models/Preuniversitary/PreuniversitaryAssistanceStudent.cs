using System;

namespace AKDEMIC.ENTITIES.Models.Preuniversitary
{
    public class PreuniversitaryAssistanceStudent
    {
        public Guid Id { get; set; }
        public Guid PreuniversitaryAssistanceId { get; set; }
        public PreuniversitaryAssistance PreuniversitaryAssistance { get; set; }
        public PreuniversitaryUserGroup PreuniversitaryUserGroup { get; set; }
        public Guid PreuniversitaryUserGroupId { get; set; }
        public bool IsAbsent { get; set; } = false;
        public bool Delay { get; set; }
    }
}

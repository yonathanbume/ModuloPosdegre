using System;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Templates
{
    public class PreuniversitaryUserGroupTemplate
    {
        public string Name { get; set; }

        public string UserName { get; set; }

        public string Dni { get; set; }

        public Guid Id { get; set; }

        public bool IsAbsent { get; set; }
    }
}

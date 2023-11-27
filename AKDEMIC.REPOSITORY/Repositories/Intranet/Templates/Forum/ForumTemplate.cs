using System;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Forum
{
    public class ForumTemplate
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int Rol { get; set; }
        public Guid[] Careers { get; set; }
    }
}

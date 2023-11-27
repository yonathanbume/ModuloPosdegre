using System;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Templates
{
    public class InvestigationUsers
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public bool HasRole { get; set; }
        public string Career { get; set; }
        public Guid CareerId { get; set; }
        public Guid FacultyId { get; set; }
    }
}

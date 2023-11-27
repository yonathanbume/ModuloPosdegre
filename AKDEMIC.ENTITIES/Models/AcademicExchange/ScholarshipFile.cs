using System;

namespace AKDEMIC.ENTITIES.Models.AcademicExchange
{
    public class ScholarshipFile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UrlFile { get; set; }
        public Guid ScholarshipId { get; set; }
        public Scholarship Scholarship { get; set; }
    }
}

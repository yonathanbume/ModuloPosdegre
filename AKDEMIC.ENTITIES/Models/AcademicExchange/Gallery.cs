using System;

namespace AKDEMIC.ENTITIES.Models.AcademicExchange
{
    public class Gallery
    {
        public Guid Id { get; set; }
        public Guid ScholarshipId { get; set; }
        public Scholarship Scholarship { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
    }
}

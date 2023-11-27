using System;

namespace AKDEMIC.ENTITIES.Models.AcademicExchange
{
    public class PostulationFile
    {
        public Guid Id { get; set; }
        public string UrlFile { get; set; }
        public Guid PostulationId { get; set; }
        public Postulation Postulation { get; set; }
    }
}

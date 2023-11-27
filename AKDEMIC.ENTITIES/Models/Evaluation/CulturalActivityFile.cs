using System;

namespace AKDEMIC.ENTITIES.Models.Evaluation
{
    public class CulturalActivityFile
    {
        public Guid Id { get; set; }
        public Guid CulturalActivityId { get; set; }
        public CulturalActivity CulturalActivity { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}

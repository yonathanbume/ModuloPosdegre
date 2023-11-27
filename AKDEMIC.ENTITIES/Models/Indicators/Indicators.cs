using System;

namespace AKDEMIC.ENTITIES.Models.Indicators
{
    public class Indicators
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal MindRank { get; set; }
        public decimal MediumdRank { get; set; }
        public decimal MaxdRank { get; set; }
    }
}

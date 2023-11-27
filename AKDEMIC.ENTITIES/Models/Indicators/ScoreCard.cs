using System;

namespace AKDEMIC.ENTITIES.Models.Indicators
{
    public class ScoreCard
    {
        public Guid Id { get; set; }
        public byte Balance { get; set; } 
        public byte Detail { get; set; }
        public bool Quantity { get; set; } 
        public decimal Value { get; set; }
    }
}

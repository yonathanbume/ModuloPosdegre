using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class Debt
    {
        public Guid Id { get; set; }
        public string Concept { get; set; }
        public string Commentary { get; set; }
        public byte DocumentType { get; set; }
        public string DocumentSerie { get; set; }
        public int DocumentNumber { get; set; }
        public decimal Amount { get; set; }
    }
}

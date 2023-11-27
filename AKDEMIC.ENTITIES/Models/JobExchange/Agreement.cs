using System;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class Agreement
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public string Institution { get; set; }
        public string Type { get; set; }
        public string Modality { get; set; }
        public string Name { get; set; }
        public string Objective { get; set; }
        public string Resolution { get; set; }
        public string Coordinator { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsUndefined { get; set; }
        public string Document { get; set; }
    }
}

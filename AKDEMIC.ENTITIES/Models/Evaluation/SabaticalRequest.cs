using System;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Evaluation
{
    public class SabaticalRequest
    {
        public Guid Id { get; set; }
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime SubmitDate { get; set; }
        public string Justification { get; set; }
        public byte Status { get; set; }
    }
}

using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class Prescription
    {
        public Guid Id { get; set; }
        public Guid TopicConsultId { get; set; }
        public string Description { get; set; }
        public string MedicalIndication { get; set; }
        public DateTime CurrentDate { get; set; }
        public TopicConsult TopicConsult { get; set; }
    }
}

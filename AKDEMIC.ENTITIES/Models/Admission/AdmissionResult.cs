using System;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class AdmissionResult
    {
        public Guid Id { get; set; }

        public Guid PostulantId { get; set; }

        public bool WasAccepted { get; set; } = false;

        public decimal Grade { get; set; }

        public Postulant Postulant { get; set; }
    }
}

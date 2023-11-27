using System;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class VocationalTestAnswerCareerPostulant
    {
        public Guid Id { get; set; }
        public Guid VocationalTestAnswerCareerId { get; set; }
        public Guid PostulantId { get; set; }
        public VocationalTestAnswerCareer VocationalTestAnswerCareer { get; set; }
        public Postulant Postulant { get; set; }
    }
}

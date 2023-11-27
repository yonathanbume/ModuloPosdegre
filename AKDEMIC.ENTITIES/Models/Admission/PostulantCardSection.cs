using System;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class PostulantCardSection
    {
        public Guid Id { get; set; }
        public Guid AdmissionTypeId { get; set; }
        public AdmissionType  AdmissionType  { get; set; }
        public bool IsVisible { get; set; }
        public bool IsRequired { get; set; }        
        public int SectionId { get; set; }
    }   
}

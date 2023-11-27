using System;

namespace AKDEMIC.ENTITIES.Models.AcademicExchange
{
    public class AcademicAgreementType
    {
        public Guid Id { get; set; }
        
        public Guid AcademicAgreementId { get; set; }
        public AcademicAgreement AcademicAgreement { get; set; }

        public Guid AgreementTypeId { get; set; }
        public AgreementType AgreementType { get; set; }
    }
}

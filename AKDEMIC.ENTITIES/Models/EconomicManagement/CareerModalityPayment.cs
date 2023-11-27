using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class CareerModalityPayment
    {
        public Guid Id { get; set; }

        public Guid CareerId { get; set; }

        public Guid AdmissionTypeId { get; set; }

        public byte NumberOfFees { get; set; }

        public bool IsCostPerCredit { get; set; }

        public Guid ConceptId { get; set; }

        public Career Career { get; set; }

        public AdmissionType AdmissionType { get; set; }

        public Concept Concept { get; set; }

        public ICollection<CareerModalityPaymentFee> Fees { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Templates.Postulant
{
    public class PostulantPaymentTemplate
    {
        public Guid Id { get; set; }
        public string ApplicationTerm { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public string OperationCode { get; set; }
        public string AdmissionType { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentDateText { get; set; }
    }
}

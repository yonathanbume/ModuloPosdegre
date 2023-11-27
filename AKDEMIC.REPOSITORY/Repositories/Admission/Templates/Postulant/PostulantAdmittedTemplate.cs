using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Templates.Postulant
{
    public class PostulantAdmittedTemplate
    {
        public Guid id { get; internal set; }
        public string paternalSurname { get; set; }
        public string maternalSurname { get; set; }
        public string name { get; set; }
        public string document { get; set; }
        public string documentType { get; set; }
        public string nationalityCountry { get; set; }
        public string sex { get; set; }
        public string birthDate { get; set; }
        public string maritalStatus { get; set; }
        public string email { get; set; }
        public string discapacity { get; internal set; }
        public string discapacityType { get; internal set; }
        public string applicationTerm { get; internal set; }
        public string campus { get; internal set; }
        public string career { get; internal set; }
        public string admissionType { get; internal set; }
        public decimal score { get; internal set; }
        public string accepted { get; internal set; }
        public string acceptedcareer { get; internal set; }
        public string ethnicId { get; internal set; }
        public string personalemail { get; internal set; }
        public string phone { get; internal set; }
        public string phone2 { get; internal set; }
        public bool paidadmission { get; internal set; }
        public string code { get; internal set; }
    }
}

using System;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Postulant
{
    public class PostulantResultTemplate
    {
        //datos generales
        public string Name { get;  set; }
        public string PaternalSurname { get;  set; }
        public string MaternalSurname { get;  set; }
        public string Document { get; set; }
        public string DocumentType { get; set; }
        public string NationalityCountry{ get; set; }
        public string Sex { get; set; }
        public string BirthDate { get; set; }
        public string MaritalStatus { get; set; }
        public string Phone1 { get; set; }
        public string Email { get; set; }
        public string Image { get;  set; }


        //postulacion
        public string AdmissionType { get; set; }
        public string ApplicationTerm{ get; set; }
        public DateTime? ApplicationTermCreatedAt { get; set; }
        public string Career { get; set; }
        public string AdmissionState { get; set; } //0 Pendiente , 1 Admitido , 2 No admitido
        public string PaidAdmission { get; set; }
        public string IsVerified { get; set; }

        //examen
        public string ClassRoom { get; set; }
        public string AdmissionExamDate { get; set; }

        //resultados
        public bool ResultPublished { get; set; }
        public decimal FinalScore { get; set; }
        public string OrderMerit { get; set; } 
        public string Condition { get;  set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class StudentComprobantInscriptionTemplate
    {
        public Guid StudentId { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string LastNames { get; set; }
        public string UserName { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string Document { get; set; }
        public string DocumentType { get; set; }
        public int AdmissionTermYear { get; set; }
        public string ApplicationTerm { get; set; }
        public string ModalityName { get; set; }
        public int? OrderMeritBySchool { get; set; }
        public int? OrderMeritGeneral { get; set; }
        public decimal? Score { get; set; }
        public string AdmissionTypeResolution { get; set; }
        public string Picture { get; set; }
        public string PostulantCode { get; set; }
    }
}

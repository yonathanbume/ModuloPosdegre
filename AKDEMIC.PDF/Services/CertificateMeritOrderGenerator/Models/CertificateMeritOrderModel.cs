using AKDEMIC.PDF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.PDF.Services.CertificateMeritOrderGenerator.Models
{
    public class CertificateMeritOrderModel
    {
        public string UserProcedureCode { get; set; }
        public string LogoImg { get; set; }
        public string UserLoggedIn { get; set; }
        public string HeaderText { get; set; }
        public string SubHeaderText { get; set; }
        public string QrCode { get; set; }
        public string FullName { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string UserName { get; set; }
        public string Term { get; set; }
        public int MeritOrder { get; set; }
        public int TotalMeritOrder { get; set; }
        public decimal WeightedAverageGrade { get; set; }
        public string HmltContent { get; set; }
        public UniversityInformationModel University { get; set; }
    }
}

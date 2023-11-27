using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates
{
    public class JobOfferApplicationTemplate
    {
        public Guid id { get;  set; }
        public string code { get;  set; }
        public string name { get;  set; }
        public string career { get;  set; }
        public int year { get;  set; }
        public string meritType { get;  set; }
        public string status { get;  set; }
        public byte statusType { get;  set; }
        public Guid jobOfferId { get;  set; }
        public Guid studentId { get;  set; }
        public string location { get;  set; }
        public string company { get;  set; }
        public bool hascv { get;  set; }
    }

    public class JobOfferApplicationAgreementReportTemplate
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Career { get; set; }
        public string Company { get; set; }
        public string Agreement { get; set; }
        public string Status { get; set; }
        public string ApplicationDate { get; set; }//Fecha de postulacion
        public string JobOfferEndDate { get; set; }//Fecha de fin la oferta de trabajo
        public string JobOfferStartDate { get; set; }//Fecha de inicio la oferta de trabajo
    }

    public class JobOfferApplicationExcelReportTemplate
    {
        public string Company { get; set; }
        public string Position { get; set; }
        public int ApplicationsCount { get; set; }
        public string JobOfferStartDate { get; set; }//Fecha de inicio la oferta de trabajo
        public string JobOfferEndDate { get; set; }//Fecha de fin la oferta de trabajo
    }
}

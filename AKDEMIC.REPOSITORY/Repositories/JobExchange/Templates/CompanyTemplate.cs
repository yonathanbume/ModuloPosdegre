using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates
{
    public class CompanyTemplate
    {
        public string Ruc { get; set; }
        public string FullName { get; set; }
        public string CompanyTypeName { get; set; }
        public string SectorName { get; set; }
        public string CompanySizeName { get; set; }
        public string EconomicActivityName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class CompanyReportPdf
    {
        public string ImageLogo { get; set; }
        public string HeaderText { get; set; }
        public List<CompanyTemplate> Companies { get; set; }
    }
}

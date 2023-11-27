using AKDEMIC.CORE.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates
{
    public class JobOfferTemplate
    {
        public Guid Id { get; set; }

        public string Position { get; set; }

        public string PublishDate { get; set; }

        public string Company { get; set; }

        public string WorkType { get; set; }

        public decimal Salary { get; set; }

        public string Type { get; set; }

        public string CompanyImage { get; set; }

        public int TimeLeft => (EndDate.ToDefaultTimeZone() - DateTime.UtcNow.ToDefaultTimeZone()).Days;

        public string Functions { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public int Status { get; set; }
    }
}

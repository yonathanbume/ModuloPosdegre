﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Models.JobExchange;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class CompanyInterest
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public DateTime Date { get; set; }

        [NotMapped]
        public string DateFormatted { get; set; }
    }
}

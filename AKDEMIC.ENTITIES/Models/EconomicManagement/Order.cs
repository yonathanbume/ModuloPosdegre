using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class Order : Entity, ICodeNumber
    {
        public Guid Id { get; set; }

        public decimal Cost { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public string FileName { get; set; }

        [Required]
        public int Number { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public int Status { get; set; } = 1;
        public string Title { get; set; }
        public bool Type { get; set; } = false;
        public string Observation { get; set; }
        public int FundingSource { get; set; } = 1;
        public bool IsPaid { get; set; } = false;



        [NotMapped]
        public string Code => $"REQ-{Number}";

        [NotMapped]
        public string ParsedEndDate => EndDate.ToLocalDateTimeFormat();

        [NotMapped]
        public string ParsedStartDate => StartDate.ToLocalDateTimeFormat();

        public ICollection<OrderChangeHistory> OrderChanges { get; set; }
        public ICollection<UserRequirement> UserRequirements { get; set; }
    }
}

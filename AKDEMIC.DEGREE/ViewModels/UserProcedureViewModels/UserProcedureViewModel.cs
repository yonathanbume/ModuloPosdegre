using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DEGREE.ViewModels.UserProcedureViewModels
{
    public class UserProcedureViewModel
    {
        public Guid Id { get; set; }

        public Guid ProcedureId { get; set; }
        public string UserId { get; set; }

        [Required]
        public int Status { get; set; } = 1;
        public string Comment { get; set; } = "";
        public decimal Cost { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public decimal Paid { get; set; } = 0;

        [MaxLength(8)]
        [RegularExpression("[^0-9]")]
        public string DNI { get; set; }
        
        public Dictionary<int, string> UserProcedureStatusValues { get; set; }
    }
}

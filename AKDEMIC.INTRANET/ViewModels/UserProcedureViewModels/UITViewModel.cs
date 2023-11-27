using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.ViewModels.UserProcedureViewModels
{
    public class UITViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public int Year { get; set; }
    }
}

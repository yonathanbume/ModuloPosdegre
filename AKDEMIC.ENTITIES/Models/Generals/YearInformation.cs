using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class YearInformation
    {
        public Guid Id { get; set; }
        [Required]
        public int Year { get; set; } //This should be unique
        public string Name { get; set; }
    }
}

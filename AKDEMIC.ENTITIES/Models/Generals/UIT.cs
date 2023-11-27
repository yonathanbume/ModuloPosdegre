using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class UIT
    {
        public Guid Id { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public int Year { get; set; }
    }
}

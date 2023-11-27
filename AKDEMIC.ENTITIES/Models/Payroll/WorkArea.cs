using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class WorkArea
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}

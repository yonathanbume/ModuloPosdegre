using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models
{
    public class WorkerPosition
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string AcademicDegree { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Required]
        [StringLength(50)]
        public string Dedication { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Document { get; set; }

        [Required]
        [StringLength(50)]
        public string JobTitle { get; set; }
    }
}

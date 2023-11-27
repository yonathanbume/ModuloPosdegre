using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class StudentAcademicEducation
    {
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsStudying { get; set; }

        public byte Type { get; set; } = 1;

        public int StartYear { get; set; }

        public int EndYear { get; set; }

        public Student Student { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class PostulantFamily
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Relation { get; set; }
        public string Occupation { get; set; }
        public string Workplace { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public Guid PostulantId { get; set; }

        public Postulant Postulant { get; set; }
    }
}

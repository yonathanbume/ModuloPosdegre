using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.PreprofesionalPractice
{
    public class InternshipDevelopment : Entity, ITimestamp
    {
        [Key]
        public Guid InternshipAspectId { get; set; }
        public InternshipAspect InternshipAspect { get; set; }

        [Key]
        public Guid InternshipRequestId { get; set; }
        public InternshipRequest InternshipRequest { get; set; }

        //Text Questions
        public string Description { get; set; }

        //Y/N Questions
        public bool? YNResponse { get; set; }
    }
}

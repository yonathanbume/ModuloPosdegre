using AKDEMIC.CORE.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class InternshipRequest
    {
        public Guid Id { get; set; }
        public Guid StudentExperienceId { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public StudentExperience StudentExperience { get; set; }
        public string Document { get; set; }
        public bool IsActive { get; set; } = false;
        public int ConvalidationType { get; set; }
        public int Status { get; set; } // ConstantHelpers.INTERNSHIPREQUEST.Status.PENDING

        [NotMapped]
        public string ConvalidationTypeString => ConstantHelpers.INTERNSHIPREQUEST.Type.VALUES.ContainsKey(ConvalidationType)
                            ? ConstantHelpers.INTERNSHIPREQUEST.Type.VALUES[ConvalidationType] : "Desconocido";
    }
}

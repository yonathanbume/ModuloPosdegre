using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Templates.AdmissionRequirement
{
    public class PostulantRequirementTemplate
    {
        public Guid AdmissionRequirementId { get; set; }

        public string Name { get; set; }

        public string File { get; set; }

        public bool Validated { get; set; }

        public string FileType { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class VocationalTestQuestion
    {
        public Guid Id { get; set; }
        public Guid VocationalTestId { get; set; }
        public string Description { get; set; }
        public VocationalTest VocationalTest { get; set; }
        public ICollection<VocationalTestAnswer> vocationalTestAnswers { get; set; }

    }
}

using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.PreprofesionalPractice
{
    public class PresentationLetter : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public string Code { get; set; }
        public string Workplace { get; set; }
        public string AddressedTo { get; set; }
        public string Position { get; set; }
        public int Days { get; set; }

        public string FinalDocumentUrl { get; set; }
    }
}

using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class SanctionedPostulant : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string DNI { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string FullName { get; set; }
        public string Reason { get; set; }
        public Guid? ApplicationTermId { get; set; }
        public ApplicationTerm ApplicationTerm { get; set; }
    }
}

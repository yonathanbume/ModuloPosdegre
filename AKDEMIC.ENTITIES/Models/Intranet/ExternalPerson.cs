using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ExternalPerson
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }
        public string Dni { get; set; }
        public string Phone { get; set; }
    }
}

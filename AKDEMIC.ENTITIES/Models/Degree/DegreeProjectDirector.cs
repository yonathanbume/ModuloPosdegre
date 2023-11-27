using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Degree
{
    public class DegreeProjectDirector
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surnames { get; set; }

        public string IdentificationCard { get; set; }
        public string PhoneNumber { get; set; }
        public byte Sex { get; set; }
        public Guid CountryId { get; set; }
        public Country Country { get; set; }

        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public byte CivilStatus { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
